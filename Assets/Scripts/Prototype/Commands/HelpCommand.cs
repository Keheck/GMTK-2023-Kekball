public class HelpCommand : ICommand{
    public string Run(string[] args) {
        string helpText = "";
        int i = 0;
        foreach (ICommand cmd in GameState.commands.Values) {
            if (i++ != 0) helpText += "\n";
            helpText += cmd.GetUsage();
        }
        return helpText;
    }

    public string GetUsage() {
        return "help - shows the usage of all available commands";
    }
}