class RespawnPlayerTask : Task {

    public RespawnPlayerTask(int timeSinceSent, int timeLimit, Player player) : base(timeSinceSent, timeLimit) {
        this.targetPlayer = player;
    }

    public override string GetDescription() {
        return $"Respawn {targetPlayer.name}";
    }

    public override bool IsSatisfied() {
        return targetPlayer.isAlive;
    }

    public override bool IsViolated() {
        return !GameState.players.Contains(targetPlayer);
    }
}