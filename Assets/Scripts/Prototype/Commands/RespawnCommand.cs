using UnityEngine;

public class RespawnCommand : ICommand{
    public string Run(string[] args) {
        if (args.Length == 1) return "Specify a player.";
        Player plr = GameState.GetConnectedPlayer(args[1]);
        if (plr is not null) {
            if (plr.isAlive = !plr.isAlive) {
                plr.health = 100;
                return $"{plr.name} respawned.";
            }
            else return $"{plr.name} is not dead.";
            
        }
        plr = GameState.GetConnectingPlayer(args[1]);
        if (plr is not null) return $"{plr.name} is not connected.";
        return $"Player '{args[1]}' not found.";
    }

    public string GetUsage() {
        return "respawn <player> - brings a player back to life.";
    }
}