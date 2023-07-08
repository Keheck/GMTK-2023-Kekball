using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameState : MonoBehaviour {


    public static GameState STATE { get; private set; } // singleton instance?

    public static List<Player> players;
    public static List<Player> connectingPlayers;
    public static List<Task> tasks;
    public static Dictionary<string, ICommand> commands;

    public AudioClip newTask, failTask, winTask;

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
        commands.Add("damage", new DamageCommand());
        commands.Add("setscore", new SetScoreCommand());
    }

    void Start() {
        GenerateTasks();
    }

    async void GenerateTasks() {
        while(true) {
            // 0: Connect placer
            // 1: Disconect player (Destroy them)
            // 2: Move player
            // 3: Damage player
            // 4: Player killed -> change score
            newTask: int taskType = Mathf.FloorToInt(Random.value * 5);
        
            switch (taskType) {
                case 0:
                    int index = Mathf.FloorToInt(Random.value * Player.PLAYER_NAMES.Count);
                    Player requester = new Player(0, Player.PLAYER_NAMES[index], 50);
                    tasks.Add(new ConnectPlayerTask((int)(Random.value * 30), 100, requester));
                    connectingPlayers.Add(requester);
                    break;
                case 1:
                    if(players.Count == 0) goto newTask;
                    index = Mathf.FloorToInt(Random.value * Player.PLAYER_NAMES.Count);
                    Player timedout = players[Mathf.FloorToInt(Random.value * players.Count)];
                    break;
                case 3:
                    if(players.Count < 2) goto newTask;
                    int index1 = Mathf.FloorToInt(Random.value * players.Count);
                    int index2 = Mathf.FloorToInt(Random.value * players.Count);
                    while(index1 == index2) index2 = Mathf.FloorToInt(Random.value * players.Count);
                    tasks.Add(new DamagePlayerTask((int)(Random.value * 20), 70, players[index1], players[index2], Mathf.FloorToInt(Random.value * 20)));
                    break;
                default:
                    goto newTask;
            }

            AudioManager.PlaySound(STATE.newTask);
            await UniTask.WaitUntil(() => tasks.Count < 7);
            await UniTask.Delay((int)(Random.value * 3000 + 3000));
        }
    }

    // takes commands from the player, parses them, and executes them, returning any relevant results
    public static string SendCommand(string command) {
        string[] args = command.Trim('\n').Split(' ');
        if (args.Length == 0) return "";
        if (!commands.ContainsKey(args[0])) return $"Unrecognized command: '{args[0]}' Type 'help' for a list of commands.";
        // return the value that is returned by the command, and pass in any arguments
        string result = commands[args[0]].Run(args);

        for(int i = tasks.Count - 1; i >= 0; i--) {
            if(tasks[i].IsViolated()) {
                AudioManager.PlaySound(STATE.failTask);
                return "YOU FAILED!";
            }
            if(tasks[i].IsSatisfied()) {
                AudioManager.PlaySound(STATE.winTask);
                tasks.RemoveAt(i);
            }
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