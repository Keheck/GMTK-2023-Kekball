using UnityEngine;

public class LookupCommand : ICommand{
    public string Run(string[] args) {
        if (args.Length == 1) return "Specify a player.";
        Player plr = GameState.GetConnectedPlayer(args[1]);
        if (plr is not null) return $"Player {plr.name}:\nhp={plr.health}\nping={plr.ping}ms\nscore={plr.score} points";
        plr = GameState.GetConnectingPlayer(args[1]);
        if (plr is not null) return $"Unconnected player {plr.name}:\nhp={plr.health}\nping={plr.ping}ms\nscore={plr.score} points";
        return $"Player '{args[1]}' not found.";
    }

    public string GetUsage() {
        return "lookup <player> - displays information on a player";
    }
}