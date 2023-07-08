using UnityEngine;

public class DamagePlayerTask: Task {
    public int previousHealth;
    public int amount;
    public Player destinationPlayer;

    public DamagePlayerTask(int timeSinceSent, int timeLimit, Player destination, int damage) : base(timeSinceSent, timeLimit) {
        this.previousHealth = destination.health;
        this.amount = damage;
        this.destinationPlayer = destination;
    }

    public override string GetDescription() {
        return $"Apply {amount} damage to {destinationPlayer.name}.";
    }

    public override bool IsSatisfied() {
        bool result = destinationPlayer.health == (int)Mathf.Max(0, previousHealth - amount);

        return result;
    }

    public override bool IsViolated() {
        return false;//destinationPlayer.health != previousHealth && destinationPlayer.health != (int)Mathf.Max(0, amount - previousHealth);
    }
}