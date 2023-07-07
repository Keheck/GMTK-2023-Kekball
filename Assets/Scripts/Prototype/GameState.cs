using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    public static GameState STATE { get; private set; } // singleton instance?

    public static List<Player> players;
    public static List<Task> tasks;
    public static Dictionary<string, ICommand> commands;

    private void Awake() {
        if (STATE is null) STATE = this;
        else Destroy(this);
        DontDestroyOnLoad(this);

        players = new List<Player>();
        tasks = new List<Task>();
        commands = new Dictionary<string, ICommand>();
        commands.Add("help", new HelpCommand());
    }

    // takes commands from the player, parses them, and executes them, returning any relevant results
    public static string SendCommand(string command) {
        string[] args = command.Trim('\n').Split(' ');
        if (args.Length == 0) return "";
        if (!commands.ContainsKey(args[0])) return $"Unrecognized command: '{args[0]}' Type 'help' for a list of commands.";
        // return the value that is returned by the command, and pass in any arguments
        return commands[args[0]].Run(args);
    }
}