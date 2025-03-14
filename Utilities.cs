using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using System.Collections.Generic;

namespace CS2EasyFlexChat
{
    public static class Utilities
    {
        public static bool IsOnCooldown(string command, Dictionary<string, DateTime> cooldowns, int cooldownSeconds, out int secondsLeft)
        {
            if (cooldowns.TryGetValue(command, out var lastUse) && cooldownSeconds > 0)
            {
                var timeSince = (DateTime.Now - lastUse).TotalSeconds;
                if (timeSince < cooldownSeconds)
                {
                    secondsLeft = (int)(cooldownSeconds - timeSince);
                    return true;
                }
            }
            secondsLeft = 0;
            return false;
        }

        public static void RegisterCommandTriggers(BasePlugin plugin, MessageConfig msg, CommandInfo.CommandCallback handler)
        {
            var triggers = new List<string> { $"css_{msg.Command}" };
            triggers.AddRange(msg.Aliases.Select(a => $"css_{a}"));
            foreach (var trigger in triggers)
            {
                plugin.AddCommand(trigger, $"Runs !{msg.Command}", handler);
            }
        }

        public static string ReplaceVariables(CCSPlayerController? player, string text, string? arg = null)
        {
            return text.Replace("{player}", player?.PlayerName ?? "Server")
                       .Replace("{time}", DateTime.UtcNow.AddHours(8).ToString("HH:mm:ss"))
                       .Replace("{score}", player?.Score.ToString() ?? "0")
                       .Replace("{target_player}", arg ?? "")
                       .Replace("{guess}", arg ?? "")
                       .Replace("{count}", arg ?? "")
                       .Replace("{player_choice}", arg ?? "")
                       .Replace("{bot_choice}", "")
                       .Replace("{result}", "")
                       .Replace("{roll}", "");
        }

        public static string ParseColors(string text)
        {
            var colorMap = new Dictionary<string, string>
            {
                ["{red}"] = "\x07FF0000", ["{green}"] = "\x077FFF00", ["{blue}"] = "\x0700FFFF",
                ["{yellow}"] = "\x07FFFF00", ["{orange}"] = "\x07FFA500", ["{white}"] = "\x01",
                ["{purple}"] = "\x078000FF", ["{cyan}"] = "\x0700FFFF"
            };
            foreach (var color in colorMap) text = text.Replace(color.Key, color.Value);
            return text;
        }

        public static List<CCSPlayerController> GetTargets(string target, List<string> targetList, Dictionary<string, List<string>> groups)
        {
            var players = CounterStrikeSharp.API.Utilities.GetPlayers().Where(p => p.IsValid && !p.IsBot).ToList();
            return target switch
            {
                "@all" => players,
                "specific" => players.Where(p => targetList.Contains(p.SteamID.ToString())).ToList(),
                _ => players.Where(p => groups.TryGetValue(target, out var ids) && ids.Contains(p.SteamID.ToString())).ToList()
            };
        }

        public static bool CanCall(CCSPlayerController player, string permission, List<string> targetList, Dictionary<string, List<string>> groups)
        {
            return permission switch
            {
                "@all" => true,
                "specific" => targetList.Contains(player.SteamID.ToString()),
                _ => groups.TryGetValue(permission[1..], out var ids) && ids.Contains(player.SteamID.ToString())
            };
        }

        public static bool IsAdmin(CCSPlayerController player, Dictionary<string, List<string>> groups)
        {
            return groups.TryGetValue("admin", out var ids) && ids.Contains(player.SteamID.ToString());
        }

        public static void SendMessage(MessageConfig msg, string text, List<CCSPlayerController> targets)
        {
            foreach (var target in targets)
            {
                if (msg.IsPrivate && msg.DisplayType == "chat") target.PrintToChat(text);
                else if (msg.DisplayType == "chat") Server.PrintToChatAll(text);
                else if (msg.DisplayType == "center") target.PrintToCenter(text);
                else if (msg.DisplayType == "hint") target.PrintToChat(text);
                else if (msg.DisplayType == "console") target.PrintToConsole(text);
            }
        }
    }
}