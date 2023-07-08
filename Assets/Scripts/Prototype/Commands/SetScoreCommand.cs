using UnityEngine;

public class SetScoreCommand : ICommand {
    public string Run(string[] args) {
        if (args.Length == 1) {
            GameState.ErrorSound();
            return "Specify a player.";
        }
        if (args.Length == 2) {
            GameState.ErrorSound();
            return "Specify an amount.";
        }
        Player plr = GameState.GetConnectingPlayer(args[1]);
        if (plr is not null) {
            GameState.ErrorSound();
            return $"{plr.name} is not connected.";
        }
        plr = GameState.GetConnectedPlayer(args[1]);
        if (plr is null) {
            GameState.ErrorSound();
            return $"Player '{args[1]}' not found.";
        }
        if (int.TryParse(args[2], out int amount)) {
            plr.score = amount;
            return $"{plr.name} now has a score of {amount}.";
        }
        GameState.ErrorSound();
        return $"Invalid amount: '{args[2]}'";
    }

    public string GetUsage() {
        return "setscore <player> <amount> - sets the score of a player";
    }
}