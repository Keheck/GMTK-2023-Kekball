using UnityEngine;

public class TimeoutPlayerTask : Task {

    public TimeoutPlayerTask(Player player): base(0, Random.Range(10, 20)) {
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