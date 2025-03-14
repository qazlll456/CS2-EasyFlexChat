using System.Collections.Generic;

namespace CS2EasyFlexChat
{
    public class MessageConfig
    {
        public string Command { get; set; } = "";
        public List<string> Aliases { get; set; } = new();
        public List<string> Messages { get; set; } = new();
        public string Sound { get; set; } = "";
        public int Timer { get; set; } = 0;
        public string Prefix { get; set; } = "";
        public bool Enabled { get; set; } = true;
        public int MinPlayers { get; set; } = 0;
        public int Delay { get; set; } = 0;
        public bool Log { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        public string DisplayType { get; set; } = "chat";
        public string Permission { get; set; } = "@all";
        public string Target { get; set; } = "@all";
        public List<string> TargetList { get; set; } = new();
        public int Cooldown { get; set; } = 1;
        public string PermissionErrorMessage { get; set; } = "You have no right to use this command!";
    }

    public class Config
    {
        public Dictionary<string, List<string>> Groups { get; set; } = new();
        public List<MessageConfig> Messages { get; set; } = new();
    }
}