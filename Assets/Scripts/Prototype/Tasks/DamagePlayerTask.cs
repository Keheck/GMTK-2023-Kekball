using UnityEngine;

public class DamagePlayerTask : Task {
    public int amount;
    private bool completed = false;

    public DamagePlayerTask(int timeSinceSent, int timeLimit, Player targetPlayer, int damage) : base(timeSinceSent, timeLimit) {
        this.amount = damage;
        this.targetPlayer = targetPlayer;
        // when the player takes damage, check if it was the right amount and complete the task
        targetPlayer.OnDamaged += (damageAmount) => completed = damageAmount == amount;
    }

    public override string GetDescription() {
        return $"Apply {amount} damage to {targetPlayer.name}.";
    }

    public override bool IsSatisfied() {
        return completed;
    }

    public override bool IsViolated() {
        return !GameState.players.Contains(targetPlayer);
    }
}