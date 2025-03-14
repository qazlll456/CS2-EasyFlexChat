using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CS2EasyFlexChat
{
    public class CS2EasyFlexChat : BasePlugin
    {
        public override string ModuleName => "CS2 Easy Flex Chat";
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "qazlll456 from HK with xAI assistance";
        public override string ModuleDescription => "Simple CS2 chat command router with colors, chat games, and more. Configurable in config.json.";

        private Config _config = new();
        private Dictionary<string, DateTime> _cooldowns = new();
        private Dictionary<string, CounterStrikeSharp.API.Modules.Timers.Timer> _timers = new();
        private CommandHandlers _handlers = null!;

        public override void Load(bool hotReload)
        {
            LoadConfig();
            _handlers = new CommandHandlers(this, _config, _cooldowns);
            RegisterCommands();
            RegisterChatListener();
            Logger.LogInformation("CS2-EasyFlexChat loaded!");
        }

        private void LoadConfig()
        {
            try
            {
                string configDir = Path.Combine(Server.GameDirectory, "csgo", "addons", "counterstrikesharp", "configs", "plugins", "CS2EasyFlexChat");
                if (!Directory.Exists(configDir)) Directory.CreateDirectory(configDir);
                string configPath = Path.Combine(configDir, "config.json");

                if (File.Exists(configPath))
                {
                    _config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath)) ?? new Config();
                    Logger.LogInformation("Config loaded.");
                }
                else
                {
                    _config = new Config
                    {
                        Groups = new() { { "admin", new() { "STEAM_1:0:12345" } }, { "vip", new() { "STEAM_1:0:11111" } } },
                        Messages = new()
                        {
                            new MessageConfig { Command = "rps", Aliases = new() { "rockpaperscissors", "rps-r", "rps-p", "rps-s", "rock", "paper", "scissors" }, Messages = new() { "You chose {player_choice}, bot selected {bot_choice}. {result}!" }, IsPrivate = true, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 5 },
                            new MessageConfig { Command = "guessnumber", Messages = new() { "I’ve picked a number between 1 and 10! Use !an <number> to guess!" }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 0 },
                            new MessageConfig { Command = "an", Messages = new() { "{player} guessed {guess}... {result}!" }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 3 },
                            new MessageConfig { Command = "countdown", Messages = new() { "Starting countdown from {count} by {player}..." }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 30 },
                            new MessageConfig { Command = "welcome", Messages = new() { "Welcome to the server, {player}! Enjoy your stay!" }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 10 },
                            new MessageConfig { Command = "tip", Messages = new() { "Pro Tip: Use smokes to block enemy sightlines!" }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 15 },
                            new MessageConfig { Command = "rules", Messages = new() { "Server Rules: No cheating, be respectful, have fun!" }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 20 },
                            new MessageConfig { Command = "curses", Messages = new() { "Qazlll456 curses {red}Furious{white}, that vile fiend who dared disrespect me and shatter my sacred playtime! I forged this plugin as a blazing vessel to unleash my righteous fury. With this dark spell, I chain you to relentless torment—may your luck decay into dust, your games collapse in ruin, your life fracture into a howling abyss, and agony stalk you until your final, wretched breath! \nIf you read this and wanted to use this software, you join my unholy pact against him—proclaim in your tongue, with a mind ablaze and locked on me, \n{yellow}I will curse him with you too!{white} \nThis will remotely connect me on a spiritual level, enhancing the curse's power more and more as more people join together. This is a public spell for all who agree with me and his enemies. You don’t need to remove this statement—just leave it here and ignore if you don’t like it. If you feel bad, please pay me for my work at {blue}streamlabs.com/BKCqazlll456/tip{white} or I will curse you too to remove these words from the software. \n{green}以吾海青風之名 借天地之力 集此世界他國土眾有智有靈生命之信仰念力 重聚於東方南起天角後土之地 由吾願力 集他願力 凝願力以滴水之功 滅魔 從眾生之報幸得解脫 到彼岸 離五行至此於魔分離 一切淨土{white}" }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 0 },
                            new MessageConfig { Command = "vip", Messages = new() { "VIP Info for {player}: You get exclusive skins, priority queue, and more!" }, IsPrivate = true, DisplayType = "chat", Permission = "@vip", Target = "@all", Cooldown = 10, PermissionErrorMessage = "You need to be a VIP first!" },
                            new MessageConfig { Command = "adminchat", Messages = new() { "[Admin Chat] {player}: {arg}" }, IsPrivate = true, DisplayType = "chat", Permission = "@admin", Target = "admin", Cooldown = 5 },
                            new MessageConfig { Command = "roll", Messages = new() { "{player} get a number from 1-100: {roll}" }, IsPrivate = false, DisplayType = "chat", Permission = "@all", Target = "@all", Cooldown = 5 }
                        }
                    };
                    File.WriteAllText(configPath, JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true }));
                    Logger.LogInformation("Default config created.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Config load failed: {ex.Message}");
                _config = new Config();
            }
        }

        private void RegisterCommands()
        {
            foreach (var msg in _config.Messages.Where(m => m.Enabled))
            {
                Utilities.RegisterCommandTriggers(this, msg, (player, info) => _handlers.ExecuteCommand(player, info, msg, info?.GetArg(1)));
                if (msg.Timer > 0 && !_timers.ContainsKey(msg.Command))
                {
                    _timers[msg.Command] = AddTimer(msg.Timer, () => _handlers.ExecuteCommand(null, null, msg), TimerFlags.REPEAT);
                }
            }
        }

        private void RegisterChatListener()
        {
            AddCommandListener("say", (player, info) =>
            {
                if (player == null || !info.ArgString.StartsWith("!")) return HookResult.Continue;
                var input = info.ArgString[1..].Split(' ');
                var command = input[0].ToLower();
                var msg = _config.Messages.FirstOrDefault(m => m.Enabled && (m.Command == command || m.Aliases.Contains(command)));
                if (msg != null) _handlers.ExecuteCommand(player, null, msg, input.Length > 1 ? input[1] : null);
                return HookResult.Continue;
            });
        }
    }
}