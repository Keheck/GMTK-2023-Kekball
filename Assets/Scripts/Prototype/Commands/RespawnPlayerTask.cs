class RespawnPlayerTask : Task
{
    public Player player;

    public RespawnPlayerTask(int timeSinceSent, int timeLimit, Player player) : base(timeSinceSent, timeLimit) {
        this.player = player;
    }

    public override string GetDescription() {
        return $"Respawn {player.name}";
    }

    public override bool IsSatisfied() {
        return GameState.players.Contains(player);
    }

    public override bool IsViolated() {
        return false;
    }
}