using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;

namespace CS2EasyFlexChat
{
    public class CommandHandlers
    {
        private readonly BasePlugin _plugin;
        private readonly Config _config;
        private readonly Dictionary<string, DateTime> _cooldowns;
        private (int Number, DateTime StartTime)? _guessGame;

        public CommandHandlers(BasePlugin plugin, Config config, Dictionary<string, DateTime> cooldowns)
        {
            _plugin = plugin;
            _config = config;
            _cooldowns = cooldowns;
        }

        public void ExecuteCommand(CCSPlayerController? player, CommandInfo? info, MessageConfig msg, string? arg = null)
        {
            if (player != null && !Utilities.CanCall(player, msg.Permission, msg.TargetList, _config.Groups))
            {
                player.PrintToChat(msg.PermissionErrorMessage);
                return;
            }

            if (CounterStrikeSharp.API.Utilities.GetPlayers().Count() < msg.MinPlayers)
            {
                player?.PrintToChat($"Need {msg.MinPlayers} players!");
                return;
            }

            if (Utilities.IsOnCooldown(msg.Command, _cooldowns, msg.Cooldown, out int secondsLeft))
            {
                player?.PrintToChat($"Wait {secondsLeft}s!");
                return;
            }

            string? text = null;
            if (msg.Command == "rps")
            {
                text = HandleRps(player, arg, msg);
                if (text == null) return;
            }
            else if (msg.Command == "guessnumber")
            {
                text = HandleGuessNumberStart(player, msg.Messages[0]);
                if (text == null) return;
            }
            else if (msg.Command == "an")
            {
                text = HandleGuessNumberAnswer(player, arg, msg.Messages[0]);
                if (text == null) return;
            }
            else if (msg.Command == "countdown")
            {
                HandleCountdown(player, arg, msg);
                return;
            }
            else if (msg.Command == "adminchat")
            {
                text = msg.Messages[0].Replace("{arg}", arg ?? "");
            }
            else if (msg.Command == "roll")
            {
                text = HandleRoll(player, msg.Messages[0]);
                if (text == null) return;
            }
            else
            {
                text = msg.Messages[Random.Shared.Next(msg.Messages.Count)];
            }

            text = text ?? "";
            text = Utilities.ReplaceVariables(player, text, arg ?? "");
            text = Utilities.ParseColors($"{msg.Prefix}{text}");

            if (msg.Delay > 0)
            {
                _plugin.AddTimer(msg.Delay, () => Utilities.SendMessage(msg, text, Utilities.GetTargets(msg.Target, msg.TargetList, _config.Groups)));
            }
            else
            {
                Utilities.SendMessage(msg, text, Utilities.GetTargets(msg.Target, msg.TargetList, _config.Groups));
            }

            if (!string.IsNullOrEmpty(msg.Sound))
            {
                Server.ExecuteCommand($"play {msg.Sound}");
            }

            if (msg.Log)
            {
                File.AppendAllText(Path.Combine(_plugin.ModuleDirectory, "flexchat.log"),
                    $"{DateTime.Now}: {player?.PlayerName ?? "Server"} used !{msg.Command}\n");
            }

            _cooldowns[msg.Command] = DateTime.Now;
        }

        private string? HandleRps(CCSPlayerController? player, string? arg, MessageConfig msg)
        {
            string safeArg = arg ?? "";
            string playerChoice = safeArg.ToLower() switch
            {
                "r" when msg.Aliases.Contains("rps-r") => "rock",
                "p" when msg.Aliases.Contains("rps-p") => "paper",
                "s" when msg.Aliases.Contains("rps-s") => "scissors",
                "rock" => "rock",
                "paper" => "paper",
                "scissors" => "scissors",
                _ => msg.Command.ToLower() switch
                {
                    "rock" => "rock",
                    "paper" => "paper",
                    "scissors" => "scissors",
                    _ => ""
                }
            };
            if (string.IsNullOrEmpty(playerChoice))
            {
                player?.PrintToChat("Use !rps <rock/paper/scissors>, !rock, !paper, !scissors, !rps-r, !rps-p, or !rps-s!");
                return null;
            }

            var choices = new[] { "rock", "paper", "scissors" };
            var botChoice = choices[Random.Shared.Next(3)];
            var result = (playerChoice, botChoice) switch
            {
                ("rock", "scissors") or ("paper", "rock") or ("scissors", "paper") => "You win",
                ("rock", "paper") or ("paper", "scissors") or ("scissors", "rock") => "I win",
                _ => "Tie"
            };
            return msg.Messages[0].Replace("{player_choice}", playerChoice)
                                  .Replace("{bot_choice}", botChoice)
                                  .Replace("{result}", result);
        }

        private string? HandleGuessNumberStart(CCSPlayerController? player, string messageTemplate)
        {
            if (!_guessGame.HasValue || (DateTime.Now - _guessGame.Value.StartTime).TotalSeconds > 30)
            {
                _guessGame = (Random.Shared.Next(1, 11), DateTime.Now);
                return Utilities.ReplaceVariables(player, messageTemplate);
            }
            return null;
        }

        private string? HandleGuessNumberAnswer(CCSPlayerController? player, string? arg, string messageTemplate)
        {
            if (!_guessGame.HasValue || (DateTime.Now - _guessGame.Value.StartTime).TotalSeconds > 30)
            {
                player?.PrintToChat("No game active! Use !guessnumber to start one.");
                return null;
            }

            if (!int.TryParse(arg, out int guess) || guess < 1 || guess > 10)
            {
                player?.PrintToChat("Guess a number between 1 and 10!");
                return null;
            }

            var target = _guessGame.Value.Number;
            var result = guess == target ? "Correct! You win!" : guess > target ? "Too high!" : "Too low!";
            if (guess == target) _guessGame = null;
            return messageTemplate.Replace("{player}", player?.PlayerName ?? "Someone")
                                 .Replace("{guess}", guess.ToString())
                                 .Replace("{result}", result);
        }

        private string? HandleRoll(CCSPlayerController? player, string messageTemplate)
        {
            if (player == null) return null;
            int roll = Random.Shared.Next(1, 101);
            return messageTemplate.Replace("{roll}", roll.ToString());
        }

        private void HandleCountdown(CCSPlayerController? player, string? arg, MessageConfig msg)
        {
            if (!int.TryParse(arg, out int count) || count < 1 || count > 10)
            {
                player?.PrintToChat("Use !countdown <1-10>!");
                return;
            }

            var text = Utilities.ParseColors($"{msg.Prefix}Starting countdown from {count} by {player?.PlayerName ?? "Someone"}...");
            Utilities.SendMessage(msg, text, Utilities.GetTargets(msg.Target, msg.TargetList, _config.Groups));
            for (int i = count; i > 0; i--)
            {
                int current = i;
                _plugin.AddTimer(count - i + 1, () => Server.PrintToChatAll($"{current}..."));
            }
            _plugin.AddTimer(count + 1, () => Server.PrintToChatAll("Go!"));
            _cooldowns[msg.Command] = DateTime.Now.AddSeconds(count);
        }
    }
}