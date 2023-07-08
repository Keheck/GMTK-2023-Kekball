using UnityEngine;

public class DisconnectCommand : ICommand{
    public string Run(string[] args) {
        if (args.Length == 1) {
            GameState.ErrorSound();
            return "Specify a player.";
        }
        Player plr = GameState.GetConnectedPlayer(args[1]);
        if (plr is not null) {
            GameState.players.Remove(plr);
            return $"{plr.name} has left the game.";
        }
        plr = GameState.GetConnectingPlayer(args[1]);
        GameState.ErrorSound();
        if (plr is not null) return $"{plr.name} is not connected.";
        return $"Player '{args[1]}' not found.";
    }

    public string GetUsage() {
        return "disconnect <player> - disconnects a connected player";
    }
}