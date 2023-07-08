public class TimeoutPlayerTask : Task
{
    public Player player;

    public TimeoutPlayerTask(Player player): base(0, 100) {
        this.player = player;
    }

    public override string GetDescription() {
        return $"{player.name} timed out. Disconnect {player.name}.";
    }

    public override bool IsSatisfied() {
        return !GameState.players.Contains(player);
    }

    public override bool IsViolated() {
        return false;
    }
}