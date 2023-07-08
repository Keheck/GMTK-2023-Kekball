using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameState : MonoBehaviour {

    public static GameState STATE { get; private set; } // singleton instance?

    public static List<Player> players;
    public static List<Player> connectingPlayers;
    public static List<Task> tasks;
    public static Dictionary<string, ICommand> commands;

    public AudioClip newTask, failTask, winTask, unknownCommand;
    static int playerId = 0;

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
        //commands.Add("setscore", new SetScoreCommand());
        commands.Add("disconnect", new DisconnectCommand());
        commands.Add("respawn", new RespawnCommand());
    }

    void Start() {
        GenerateTasks();
    }

    async void GenerateTasks() {
        // 0: Connect player
        // 1: Disconect player (Destroy them)
        // 2: TODO Move player
        // 3: Damage player
        // 4: TODO Player killed -> TODO which then creates change score task
        newTask: int taskType = (int)Random.Range(0, 5);
        switch (taskType) {
            case 0:
                // dont connect too many players
                if (players.Count + connectingPlayers.Count >= 6) goto newTask;
                // pick a new name for our new connecting player
                int index = Mathf.FloorToInt(Random.value * Player.PLAYER_NAMES.Count);
                string name = Player.PLAYER_NAMES[index];
                // make sure we dont already have a player with this name
                foreach (Player plr in players) {
                    if (plr.name == name) goto newTask;
                }
                foreach (Player plr in connectingPlayers) {
                    if (plr.name == name) goto newTask;
                }
                Player requester = new Player(playerId++, Player.PLAYER_NAMES[index], 50);
                tasks.Add(new ConnectPlayerTask((int)Random.Range(0, 30), 100, requester));
                connectingPlayers.Add(requester);
                break;
            case 1:
                // abort if there isnt a lot of players
                if(players.Count < 4) goto newTask;
                // choose a new random player
                Player timedOutPlayer = players[Random.Range(0, players.Count - 1)];
                // make sure we dont have any other tasks for this player
                foreach (Task task in tasks) {
                    if (task.targetPlayer == timedOutPlayer) goto newTask;
                }
                // found a player to time out, time them out
                if (timedOutPlayer.timedOut) goto newTask;
                timedOutPlayer.timedOut = true;
                tasks.Add(new TimeoutPlayerTask(timedOutPlayer));
                break;
            case 3:
                if(players.Count < 2) goto newTask;
                // get a new player
                Player player = players[(int)Random.Range(0, players.Count - 1)];
                // abort if we already have a task for this player
                foreach (Task task in tasks) {
                    if (task.targetPlayer == player) goto newTask;
                }
                tasks.Add(new DamagePlayerTask((int)Random.Range(0,20), 70, player, (int)Random.Range(1,110)));
                break;
            case 9999: // never called
                if (players.Count < 2) goto newTask;
                // get a new player
                Player player2 = players[(int)Random.Range(0, players.Count - 1)];
                // abort if we already have a task for this player
                foreach (Task task in tasks) {
                    if (task.targetPlayer == player2) goto newTask;
                }
                // note: probability of getting here is lower than other tasks, since we need two people
                tasks.Add(new SetScoreTask((int) Random.Range(0, 20), 90, player2, player2.score + (int)Random.Range(1, 3)));
                break;
            default:
                goto newTask;
        }
        AudioManager.PlaySound(STATE.newTask);
        await UniTask.WaitUntil(() => tasks.Count < 7);
        await UniTask.Delay((int)Random.Range(0, 3000) + 3000);
        goto newTask;
    }

    // takes commands from the player, parses them, and executes them, returning any relevant results
    public static string SendCommand(string command) {
        string[] args = command.Trim('\n').Split(' ');
        if (args.Length == 0) return "";
        if (!commands.ContainsKey(args[0])) {
            ErrorSound();
            return $"Unrecognized command: '{args[0]}' Type 'help' for a list of commands.";
        }
        // return the value that is returned by the command, and pass in any arguments
        string result = commands[args[0]].Run(args);

        for (int i = tasks.Count - 1; i >= 0; i--) {
            if (tasks[i].IsViolated()) {
                AudioManager.PlaySound(STATE.failTask);
                result += "\nTask Failed Successfully.";
                tasks.RemoveAt(i);
            }
            else if (tasks[i].IsSatisfied()) {
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

    public static void ErrorSound() {
        AudioManager.PlaySound(STATE.unknownCommand);
    }

    public static List<Task> GetTasks() {
        return tasks;
    }
}