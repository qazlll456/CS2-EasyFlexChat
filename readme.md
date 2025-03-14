CS2-EasyFlexChat
A simple and flexible chat command plugin for Counter-Strike 2 (CS2) servers using the CounterStrikeSharp framework. This plugin allows server admins to create custom chat commands with features like games, announcements, role-based permissions, and more. It’s highly configurable via a JSON file, making it easy to add or modify commands to suit your server’s needs.
Features
Custom Chat Commands: Define chat commands with custom messages, permissions, and targets.

Chat Games:
!rps: Play Rock-Paper-Scissors with the bot (supports !rock, !paper, !scissors, etc.).

!guessnumber: Start a number guessing game (1-10), with !an <number> to guess.

!roll: Roll a random number between 1 and 100.

Utility Commands:
!countdown <1-10>: Start a countdown timer.

!welcome, !tip, !rules: Display server messages.

!vip: Show VIP information (VIP-only).

!adminchat <message>: Private admin chat (admin-only).

!curses: Display a custom curse message (public).

Permission System: Restrict commands to specific roles (e.g., @all, @vip, @admin).

Cooldowns: Prevent spam with configurable cooldowns per command.

Configurable: All commands, messages, and settings are managed via a config.json file.

Logging: Optional logging of command usage to a file.

Installation
Prerequisites:
A running CS2 server.

CounterStrikeSharp installed on your server.

Download:
Clone or download this repository:

git clone https://github.com/[YourGitHubUsername]/CS2-EasyFlexChat.git

Build:
Open the project in Visual Studio or use the terminal:

cd CS2-EasyFlexChat
dotnet build

The compiled plugin will be in bin/Debug/net8.0/CS2EasyFlexChat.dll.

Deploy:
Copy the CS2EasyFlexChat.dll to your CS2 server’s plugin directory:

csgo/addons/counterstrikesharp/plugins/

Start your server. The plugin will generate a config.json file in:

csgo/addons/counterstrikesharp/configs/plugins/CS2EasyFlexChat/config.json

Configuration
The plugin is configured via config.json. Here’s an overview of the structure:
Groups: Define permission groups (e.g., admin, vip) with SteamIDs.

Messages: List of commands with their settings:
Command: The command name (e.g., rps for !rps).

Aliases: Alternate command names (e.g., ["rock", "paper"]).

Messages: The message(s) to display (supports variables like {player}, {roll}).

Permission: Who can use the command (e.g., @all, @vip, @admin).

Target: Who sees the message (e.g., @all, admin).

Cooldown: Time (in seconds) before the command can be reused.

PermissionErrorMessage: Custom error message for permission failures.

Example Config Snippet
json

{
  "Groups": {
    "admin": ["STEAM_1:0:12345"],
    "vip": ["STEAM_1:0:11111"]
  },
  "Messages": [
    {
      "Command": "roll",
      "Messages": ["{player} get a number from 1-100: {roll}"],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 5
    },
    {
      "Command": "vip",
      "Messages": ["VIP Info for {player}: You get exclusive skins, priority queue, and more!"],
      "IsPrivate": true,
      "DisplayType": "chat",
      "Permission": "@vip",
      "Target": "@all",
      "Cooldown": 10,
      "PermissionErrorMessage": "You need to be a VIP first!"
    }
  ]
}

Usage Guide
How to Use
Install the Plugin: Follow the installation steps above.

Edit Config: Open config.json to add or modify commands.

Restart Server: Reload the server to apply changes.

Use Commands: Type commands in chat starting with ! (e.g., !roll, !rps rock).

Example Commands
Fun/Game Commands:
!roll: Rolls a random number (e.g., "[Player] get a number from 1-100: 42").

!rps rock: Play Rock-Paper-Scissors (e.g., "You chose rock, bot selected scissors. You win!").

!guessnumber: Start a number guessing game (e.g., "I’ve picked a number between 1 and 10! Use !an <number> to guess!").

!an 7: Guess the number (e.g., "[Player] guessed 7... Too high!").

Server Announcements:
!welcome: "Welcome to the server, [Player]! Enjoy your stay!"

!tip: "Pro Tip: Use smokes to block enemy sightlines!"

!rules: "Server Rules: No cheating, be respectful, have fun!"

Role-Based Commands:
!vip: (VIP-only) "VIP Info for [Player]: You get exclusive skins, priority queue, and more!"

!adminchat Server reboot soon: (Admin-only, private) "[Admin Chat] [Player]: Server reboot soon".

Utility:
!countdown 5: Starts a 5-second countdown (e.g., "5... 4... 3... 2... 1... Go!").

Adding Custom Commands
Open config.json.

Add a new entry under Messages:
json

{
  "Command": "hello",
  "Messages": ["Hello, {player}!"],
  "IsPrivate": false,
  "DisplayType": "chat",
  "Permission": "@all",
  "Target": "@all",
  "Cooldown": 5
}

Restart the server. Now !hello will display "Hello, [Player]!" in chat.

Functions Overview
Chat Games:
!rps: Rock-Paper-Scissors with aliases (!rock, !paper, !scissors, !rps-r, etc.).

!guessnumber & !an: Number guessing game (1-10).

!roll: Random number generator (1-100).

Utility Commands:
!countdown: Countdown timer (1-10 seconds).

!adminchat: Private admin communication.

!vip: VIP-exclusive information.

Announcements:
!welcome, !tip, !rules, !curses: Predefined server messages.

Customization:
Supports colored messages using tags like {red}, {green}, etc.

Variables in messages: {player}, {roll}, {guess}, etc.

Configurable cooldowns, permissions, and targets.

Contributing
Feel free to fork this repository, make improvements, and submit a pull request! If you encounter bugs or have feature requests, please open an issue.
Credits
Author: qazlll456 from HK

Assisted by: xAI’s Grok 3

License
This project is licensed under the MIT License. See the LICENSE file for details.

