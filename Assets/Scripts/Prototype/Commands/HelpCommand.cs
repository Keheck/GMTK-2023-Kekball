public class HelpCommand : ICommand{
    public string Run(string[] args) {
        return @"## Commands for the player
- `help` displays help, early players will frequently reference this, and the player will be reminded to check it every time a new command is added
- `deny <id>` denies a request/task, removing it and returning an error to the client (for trick questions)
- `connect <player>` connects a player to your game
- `disconnect <player>` disconnects a player from your game 
- `damage <player> <amount>` damages a specified player by an amount
- `move <player> <x> <y>` moves a player to the specified position
- `move <player> <x> <y> <z>` implemented later by your devs, they fixed a flying cheat, vertical movement is now server side (Top 10 devs lmao)
- `lookup <object>` returns information related to the specified string (such as the damage of a weapon, or the id of a player)
- `cputemp` returns the temperature of your cpu (must check periodically [harder requests = more CPU increase? Interesting budget mechanic])
multithreading increases temperature a lot, and using backspace increases temperature
- `explode` secret command, fails the level and crashes the actual game
- `cls` clears your terminal to stay tidy (you can still get this as a task that might mess with you)";
    }
}