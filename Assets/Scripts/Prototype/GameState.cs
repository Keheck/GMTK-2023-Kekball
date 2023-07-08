using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameState : MonoBehaviour {

    public static GameState STATE { get; private set; } // singleton instance?

    public static List<Player> players;
    public static List<Player> connectingPlayers;
    public static List<Task> tasks;
    public static Dictionary<string, ICommand> commands;

    private void Awake() {
        if (STATE is null) STATE = this;
        else Destroy(this);
        DontDestroyOnLoad(this);

        players = new List<Player>();
        connectingPlayers = new List<Player>(); // connecting players will be added to the players list if connected 
        tasks = new List<Task>();
        commands = new Dictionary<string, ICommand>();
        commands.Add("help", new HelpCommand());
        commands.Add("cls", new ClearCommand());
        commands.Add("connect", new ConnectCommand()); // for when a new client requests connection
        commands.Add("lookup", new LookupCommand()); // for when a new client requests connection

        // example player, starts unconnected
        Player doe = new Player(2763, "exampleguy", 60000);
        doe.health = 50000;
        doe.score += int.MaxValue;
        connectingPlayers.Add(doe);

        tasks.Add(new ConnectPlayerTask(0, 50, doe));
        //GenerateNewTask();
    }

    public static async void GenerateNewTask() {
        // 0: Connect placer
        // 1: Disconect player (Destroy them)
        // 2: Move player
        // 3: Damage player
        // 4: Player killed -> change score
        int taskType = Mathf.FloorToInt(Random.value * 5);

        switch (taskType) {
            case 0:
                if(connectingPlayers.Count == 0) { GenerateNewTask(); return; }
                int index = Mathf.FloorToInt(Random.value * Player.PLAYER_NAMES.Count);
                Player requester = new Player(9, Player.PLAYER_NAMES[index], 50);
                break;
        }

        // Recursively call with a 1-4 second delay to generate new tasks
        await UniTask.Delay(Mathf.FloorToInt(Random.value * 3000 + 1000));
        GenerateNewTask();
    }

    // takes commands from the player, parses them, and executes them, returning any relevant results
    public static string SendCommand(string command) {
        string[] args = command.Trim('\n').Split(' ');
        if (args.Length == 0) return "";
        if (!commands.ContainsKey(args[0])) return $"Unrecognized command: '{args[0]}' Type 'help' for a list of commands.";
        // return the value that is returned by the command, and pass in any arguments
        string result = commands[args[0]].Run(args);

        for(int i = tasks.Count - 1; i >= 0; i--) {
            if(tasks[i].IsViolated()) return "YOU FAILED!";
            if(tasks[i].IsSatisfied()) tasks.RemoveAt(i);
        }

        return result;
    }

    public static void ClearUserTerminal() {
        FindObjectOfType<Terminal>().Clear();
    }

    public static Player GetConnectedPlayer(string name) {
        foreach (Player plr in GameState.players) {
            if (plr.name == name) return plr;
        }
        return null;
    }

    public static Player GetConnectingPlayer(string name) {
        foreach (Player plr in GameState.connectingPlayers) {
            if (plr.name == name) return plr;
        }
        return null;
    }
}