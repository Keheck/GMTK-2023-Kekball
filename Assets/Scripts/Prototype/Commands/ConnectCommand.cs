public class ConnectCommand : ICommand{
    public string Run(string[] args) {
        if (args.Length == 1) {
            GameState.ErrorSound();
            return "Specify a player.";
        }
        Player plr = GameState.GetConnectedPlayer(args[1]);
        if (plr is not null) {
            GameState.ErrorSound();
            return $"{plr.name} is already connected.";
        }
        plr = GameState.GetConnectingPlayer(args[1]);
        if (plr is null) {
            GameState.ErrorSound();
            return $"Player '{args[1]}' not found.";
        }
        GameState.players.Add(plr);
        GameState.connectingPlayers.Remove(plr);
        return $"{plr.name} joined the game.";
    }

    public string GetUsage() {
        return "connect <player> - connects a player to the game";
    }
}