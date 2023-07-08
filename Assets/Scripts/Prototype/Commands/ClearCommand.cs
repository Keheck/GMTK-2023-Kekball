using UnityEngine;

public class ClearCommand : ICommand{
    public string Run(string[] args) {
        GameState.ClearUserTerminal(); // need monobehaviour
        return "";
    }

    public string GetUsage() {
        return "cls - clears the screen";
    }
}