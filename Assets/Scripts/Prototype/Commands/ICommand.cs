public interface ICommand {
    // given arguments from the user, return a string to display on the terminal
    public string Run(string[] args);
    // used by help to display commands
    public string GetUsage();
}