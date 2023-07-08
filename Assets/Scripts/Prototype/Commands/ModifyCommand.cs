public class ModifyCommand : ICommand
{
    public string GetUsage()
    {
        return "modify <player> <stat> <newval> - sets <stat> of <player> to <newval>";
    }

    public string Run(string[] args)
    {
        Player player = GameState.GetConnectedPlayer(args[1]);

        if (player == null)
            return $"Could not find player {args[1]}";

        switch (args[2]) {
            case "health":
                if(!int.TryParse(args[3], out int value)) return "Value needs to be a number";
                player.health = value;
                return $"Set health of {args[1]} to {args[3]}";
            case "score":
                if(!int.TryParse(args[3], out value)) return "Value needs to be a number";
                player.score = value;
                return $"Set score of {args[1]} to {args[3]}";
            default:
                return $"There is not stat {args[2]}";
        }
    }
}