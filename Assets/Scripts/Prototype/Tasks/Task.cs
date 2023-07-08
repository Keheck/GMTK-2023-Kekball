public abstract class Task {
    public int TimeSinceSent = 0;
    public int TimeLimit = 0;

    public Task(int timeSinceSent, int timeLimit) {
        TimeSinceSent = timeSinceSent;
        TimeLimit = timeLimit;
    }

    public abstract string GetDescription();
    // True when the game state is as the task demanded
    public abstract bool IsSatisfied();
    // True when the current state of the game is the exact opposite of what the task demands (ex: Player was damaged for 5, but the task demanded 6)
    // Does not need to handle time expiration, since that will be handled in the GameState class
    public abstract bool IsViolated();
}