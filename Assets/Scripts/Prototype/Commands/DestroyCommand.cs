public class DestroyCommand : ICommand
{
    public string GetUsage()
    {
        return "destroy <player> - Remove player entity and erase their scores";
    }

    public string Run(string[] args)
    {
        if(args.Length < 2)
            return "Too little arguments";
        if(args.Length > 2)
            return "Too many arguments";

        Player player = GameState.GetConnectedPlayer(args[1]);

        if(player == null)
            return "Player not found";
        
        GameState.players.Remove(player);
        return $"Removed player {player.name}";
    }
}