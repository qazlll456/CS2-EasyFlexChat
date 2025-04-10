# CS2-EasyFlexChat
A versatile CounterStrikeSharp plugin for Counter-Strike 2 (CS2) that adds customizable chat commands, interactive games, and role-based interactions to your server.

## Overview
This plugin enhances your CS2 server with a variety of chat commands using the CounterStrikeSharp framework. Players can enjoy games like Rock-Paper-Scissors, roll random numbers, or use utility commands, while admins can manage role-specific messages. All features are configurable via a JSON file.

- **Module Name**:    CS2 Easy Flex Chat
- **Version**:        1.0.0
- **Author**:         qazlll456 from HK with xAI assistance
- **Description**:    A versatile plugin that supports chat commands like !rps, !guessnumber, !roll, and more, with role-based permissions and customization.

## Features
Custom Chat Commands: Create your own commands with configurable messages, permissions, and targets.
Chat Games:

- !rps: Play Rock-Paper-Scissors (supports !rock, !paper, !scissors, !rps-r, etc.).

- !guessnumber & !an: Start and guess a number (1-10) in a fun game.

- !roll: Roll a random number between 1 and 100.

### Utility Commands:
- !countdown: Start a countdown timer (1-10 seconds).

- !welcome, !tip, !rules: Share server messages. 

- !vip: Display VIP info (VIP-only).

- !adminchat: Private admin chat (admin-only).

- !curses: Show a custom curse message (public).

Permission System: Restrict commands to roles (@all, @vip, @admin).

Cooldowns: Prevent spam with per-command cooldowns.

Configurable: Manage all settings in config.json.

Logging: Optional logging of command usage.

## Requirements
To use this plugin, you need:
- **Counter-Strike 2 Dedicated Server**: A running CS2 server.
- **Metamod:Source**: Installed on your server for plugin support. Download from [sourcemm.net](https://www.sourcemm.net/).
- **CounterStrikeSharp**: The C# plugin framework for CS2. Download the latest version from [GitHub releases](https://github.com/roflmuffin/CounterStrikeSharp/releases) (choose the "with runtime" version if it’s your first install).

## Installation
1. **Download the Plugin**:
- Grab the latest .dll from the Releases section.
- Or clone this repository and build it yourself using dotnet build.

2. **Install on Your Server**:
- Navigate to your CS2 server’s plugin folder: /game/csgo/addons/counterstrikesharp/plugins/.
- Create a new folder named CS2EasyFlexChat.
- Copy the compiled CS2EasyFlexChat.dll (and related files like .deps.json, .pdb) into this folder.

3.Restart Your Server:
- Restart your CS2 server to load the plugin.
- Check the console for: CS2-EasyFlexChat loaded!.

## Configuration
Edit config.json to customize commands. Key fields include:
- Groups: Define roles (e.g., admin, vip) with SteamIDs.

- Messages: List of commands with settings:
  - Command: Command name (e.g., roll).

  - Aliases: Alternate names (e.g., ["rps-r", "rock"]).

  - Messages: Messages to display (e.g., ["{player} rolled: {roll}"]).

  - Permission: Who can use (e.g., @all, @vip).

  - Target: Who sees the message (e.g., @all, admin).

  - Cooldown: Cooldown in seconds.

## Example Config
```json
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
      "Command": "rps",
      "Aliases": ["rockpaperscissors", "rps-r", "rps-p", "rps-s", "rock", "paper", "scissors"],
      "Messages": ["You chose {player_choice}, bot selected {bot_choice}. {result}!"],
      "IsPrivate": true,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 5
    },
    {
      "Command": "guessnumber",
      "Messages": ["I’ve picked a number between 1 and 10! Use !an <number> to guess!"],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 0
    },
    {
      "Command": "an",
      "Messages": ["{player} guessed {guess}... {result}!"],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 3
    },
    {
      "Command": "countdown",
      "Messages": ["Starting countdown from {count} by {player}..."],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 30
    },
    {
      "Command": "welcome",
      "Messages": ["Welcome to the server, {player}! Enjoy your stay!"],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 10
    },
    {
      "Command": "tip",
      "Messages": ["Pro Tip: Use smokes to block enemy sightlines!"],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 15
    },
    {
      "Command": "rules",
      "Messages": ["Server Rules: No cheating, be respectful, have fun!"],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 20
    },
    {
      "Command": "curses",
      "Messages": ["Qazlll456 curses {red}Furious{white}, that vile fiend who dared disrespect me and shatter my sacred playtime!..."],
      "IsPrivate": false,
      "DisplayType": "chat",
      "Permission": "@all",
      "Target": "@all",
      "Cooldown": 0
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
    },
    {
      "Command": "adminchat",
      "Messages": ["[Admin Chat] {player}: {arg}"],
      "IsPrivate": true,
      "DisplayType": "chat",
      "Permission": "@admin",
      "Target": "admin",
      "Cooldown": 5
    }
  ]
}

```

## Usage
- Chat Commands: Players can type commands like !rps, !roll, or !guessnumber in chat to trigger various actions.

- Console Commands: Admins can use console commands (e.g., via RCON) to manage plugin behavior (future updates may expand this).

## Example Commands
Chat Games

!roll:
```bash
[Player] get a number from 1-100: 42
```
!rps rock:
```bash
You chose rock, bot selected scissors. You win!
```
!guessnumber:
```bash
I’ve picked a number between 1 and 10! Use !an <number> to guess!
```
!an 7:
```bash
[Player] guessed 7... Too high!
```
### Server Announcements
!welcome:
```bash
Welcome to the server, [Player]! Enjoy your stay!
```
!tip:
```bash
Pro Tip: Use smokes to block enemy sightlines!
```
!rules:
```bash
Server Rules: No cheating, be respectful, have fun!
```
!curses:
```bash
Qazlll456 curses Furious, that vile fiend who dared disrespect me...
```
### Role-Based Commands
!vip (VIP-only):
```bash
VIP Info for [Player]: You get exclusive skins, priority queue, and more!
```
!adminchat Server reboot soon (Admin-only):
```bash
[Admin Chat] [Player]: Server reboot soon
```
### Utility
!countdown 5:
```bash
Starting countdown from 5 by [Player]... 5... 4... 3... 2... 1... Go!
```
## Adding a Custom Command
To add a !hello command:
1. Open config.json.

2. Add:
```json

{
  "Command": "hello",
  "Messages": ["Hello, {player}!"],
  "IsPrivate": false,
  "DisplayType": "chat",
  "Permission": "@all",
  "Target": "@all",
  "Cooldown": 5
}
```
3. Restart your server. Now !hello will display:
```bash
Hello, [Player]!
```
## Screenshots
Here’s a screenshot of the plugin in action:
image

## Building from Source
1. Ensure you have the .NET 8 SDK installed.

2. Clone this repository:
```bash

git clone https://github.com/qazlll456/CS2-EasyFlexChat.git
```
3. Navigate to the project folder and build:
```bash

cd CS2-EasyFlexChat
dotnet build
```
4. Find the compiled files in bin/Debug/net8.0/.

## Support the Project
If you find this plugin helpful and want to support its development, 

Money, Steam games, or any valuable contribution is welcome.

[ko_fi](https://ko-fi.com/qazlll456).

[patreon](https://www.patreon.com/c/qazlll456).

[Streamlabs](https://streamlabs.com/BKCqazlll456/tip)

Your support is greatly appreciated!

## License
This project is licensed under the MIT License. See the LICENSE file for details.

## Credits
Developed by qazlll456 from Hong Kong with assistance from xAI’s Grok.

Built using the CounterStrikeSharp framework by roflmuffin.

