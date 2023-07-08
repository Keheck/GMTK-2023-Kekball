using UnityEngine;

public class DamagePlayerTask: Task {
    public int previousHealth;
    public int amount;
    public Player destinationPlayer;
    public Player sourcePlayer;

    public DamagePlayerTask(int timeSinceSent, int timeLimit, Player source, Player destination, int damage) : base(timeSinceSent, timeLimit) {
        this.previousHealth = destination.health;
        this.amount = damage;

        this.destinationPlayer = destination;
        this.sourcePlayer = source;
    }

    public override string GetDescription() {
        return $"{sourcePlayer.name} damaged {destinationPlayer.name} for {amount} damage";
    }

    public override bool IsSatisfied() {
        return destinationPlayer.health == (int)Mathf.Max(0, previousHealth - amount);
    }

    public override bool IsViolated() {
        return destinationPlayer.health != previousHealth && destinationPlayer.health != (int)Mathf.Max(0, amount - previousHealth);
    }
}