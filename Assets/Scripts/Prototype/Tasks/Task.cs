public abstract class Task {
    public float timeSinceSent = 0;
    public float timeLimit = 0;
    public Player targetPlayer = null;
    public bool expired = false;

    public Task(int timeSinceSent, int timeLimit) {
        this.timeSinceSent = timeSinceSent;
        this.timeLimit = timeLimit * 1.95f;
    }

    public abstract string GetDescription();
    // True when the game state is as the task demanded
    public abstract bool IsSatisfied();
    // True when the current state of the game is the exact opposite of what the task demands (ex: Player was damaged for 5, but the task demanded 6)
    // Does not need to handle time expiration, since that will be handled in the GameState class
    public abstract bool IsViolated();
}