public class TimeoutPlayerTask : Task {

    public TimeoutPlayerTask(Player player, int timeLimit): base(0, timeLimit) {
        this.targetPlayer = player;
    }

    public override string GetDescription() {
        return $"Disconnect {targetPlayer.name}: timed out.";
    }

    public override bool IsSatisfied() {
        return !GameState.players.Contains(targetPlayer);
    }

    public override bool IsViolated() {
        return false;
    }
}