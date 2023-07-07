using System.Collections.Generic;

public class GameState {
    public static GameState STATE { get; } = new GameState();

    public List<Player> Players { get; set; } = new List<Player>();
    public List<Task> tasks { get; set; } = new List<Task>();
    public Dictionary<string, Command> commands { get; set; } = new Dictionary<string, Command>();

    private GameState() { }

    public static void DoThings() {
        //string command = console.text;

        // commands[command.Split(' ')[0]].Parse(command.Split(' '));
    }
}