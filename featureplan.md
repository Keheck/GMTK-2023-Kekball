# [Codename here] TASK_MANAGER?

## Features plan

- ~~Take input from player typing~~
- ~~Tasks spawn on your screen telling you what to do but now how to do it~~
- Player is gradually introduced to more mechanics
- make a system for making "levels", maybe switch between scenes?
- When a task expires it increases your lag (bad score)
- too much lag and you lose the game
- survive long enough without lagging and you win
- some tasks take a few ms for you to process (Give hint to player to allow them to prioritize)
- some tasks arrive late, and are therefore more urgent
- multithreading is eventually enabled, allowing a second and eventually third task to be executed at the same time
- keyboard shortcuts
- player must manage their cpu temperature in later levels, it is the summer and the devs are broke
- scoreboard: harder requests handled faster = more points. Expired requests or incorrectly fulfilled = points deducted + player loss, lose at 0 players online
- score could also be average ms per request (lower = better? yes :thumbs_up: :ballers:)
- add joke tasks, like `brew coffee` and `fold towels` and `file taxes` where you just type the name of the task to complete it
- ~~we need a monospace font~~
- ~~we need some sounds little beepboops on text edited and tasks added/completed/failed~~
- screen shake effect? do the screen

## Commands for the player
- `help` displays help, early players will frequently reference this, and the player will be reminded to check it every time a new command is added
- `deny <id>` denies a request/task, removing it and returning an error to the client (for trick questions)
- `connect <player>` connects a player to your game
- `disconnect <player>` disconnects a player from your game 
- `damage <player> <amount>` damages a specified player by an amount
- `score <player> <newvalue>` sets a players score
- `move <player> <x> <y>` moves a player to the specified position
- `move <player> <x> <y> <z>` implemented later by your devs, they fixed a flying cheat, vertical movement is now server side (Top 10 devs lmao)
- `lookup <object>` returns information related to the specified string (such as the damage of a weapon, or the id of a player)
- `cputemp` returns the temperature of your cpu (must check periodically [harder requests = more CPU increase? Interesting budget mechanic])
multithreading increases temperature a lot, and using backspace increases temperature
- `explode` secret command, fails the level and crashes the actual game
- `cls` clears your terminal to stay tidy (you can still get this as a task that might mess with you)

## Tasks for the player ("Requests")
put a difficulty on each one, for how late in the game they should be introduced
- Join Request (e.g. "player asked to join", easy)
- Input request (e.g. "player pressed button x", medium)
    - For movement keys (wasd) just update the player's position
    - For shots, determine whether the player hit another player (how?)
- "player is not responding, should be timed out" -> Perhaps after 50 ms? (Unrealistic, but is a lot of in-game time)
- "player has willingly disconnected" [Maybe determine whether it's a legitimate disconnect or timeout? Perhaps in a comp game :3]
- "player has been shot for 20 damage" (easy)
- "player has been shot with a level 2 slug" (medium)
- Perhaps periodically ping every player in the lobby? Provide the player a list ordered by last pinged

doeball list for tonight
BUILD
~~music?~~
~~credits~~

then we need a retry menu, to either return to menu, or highlight the option to play again, and show the score from
your highest point in the game you just lost

doeball now
~~fix volume~~
build
set selection to nothing every frame
~~balance~~
~~glow text~~
flicker?