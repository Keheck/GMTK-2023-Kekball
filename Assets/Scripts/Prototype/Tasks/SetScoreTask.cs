using UnityEngine;

public class SetScoreTask : Task {
    public int amount;

    public SetScoreTask(int timeSinceSent, int timeLimit, Player targetPlayer, int targetScore) : base(timeSinceSent, timeLimit) {
        this.amount = targetScore;
        this.targetPlayer = targetPlayer;
        // when the player gains points, check if the score is now correct
        //targetPlayer.OnScoreChanged += (pointsAwarded) => completed = targetPlayer.score == amount;
    }

    public override string GetDescription() {
        return $"Set {targetPlayer.name}'s score to {amount}.";
    }

    public override bool IsSatisfied() {
        return targetPlayer.score == amount;
    }

    public override bool IsViolated() {
        return !GameState.players.Contains(targetPlayer);
    }
}