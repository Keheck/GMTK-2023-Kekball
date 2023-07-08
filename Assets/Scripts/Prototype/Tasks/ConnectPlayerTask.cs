public class ConnectPlayerTask: Task {
    public Player player;

    public ConnectPlayerTask(int since, int limit, Player player): base(since, limit) {
        this.player = player;
    }

    public override string GetDescription() {
        return $"Connect player {player.name} (id: {player.id})";
    }

    public override bool IsSatisfied() {
        return GameState.players.Contains(player);
    }

    public override bool IsViolated() {
        return false;
    }
}