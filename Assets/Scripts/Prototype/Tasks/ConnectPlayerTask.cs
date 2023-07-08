public class ConnectPlayerTask: Task {

    public ConnectPlayerTask(int since, int limit, Player player): base(since, limit) {
        this.targetPlayer = player;
    }

    public override string GetDescription() {
        return $"Connect player: {targetPlayer.name}";
    }

    public override bool IsSatisfied() {
        return GameState.players.Contains(targetPlayer);
    }

    public override bool IsViolated() {
        return false;
    }
}