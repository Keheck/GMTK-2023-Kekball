public class TimeoutPlayerTask : Task {

    public TimeoutPlayerTask(Player player): base(0, 100) {
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