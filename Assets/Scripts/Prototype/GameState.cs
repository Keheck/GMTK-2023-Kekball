using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class GameState : MonoBehaviour {

    public static GameState STATE { get; private set; } // singleton instance?
    [SerializeField] AnimationCurve difficultyCurve;

    public static List<Player> players;
    public static List<Player> connectingPlayers;
    public static List<Task> tasks;
    public static Dictionary<string, ICommand> commands;

    public AudioClip newTask, failTask, winTask, unknownCommand;
    public static int playerId = 0;
    public static int taskFrequency = 900;
    public static int score = 100;
    public static int highestScore = 100;
    public static int thisRunHigh = 100;
    public static float timeSurvived = 0;
    public static bool generateTasks = true;

    public GameObject gamePanel;
    public GameObject losePanel;

    private void Awake() {
        if (STATE is null) STATE = this;
        else Destroy(this);
        //DontDestroyOnLoad(this);

        players = new List<Player>();
        connectingPlayers = new List<Player>(); // connecting players will be added to the players list if connected 
        tasks = new List<Task>();
        commands = new Dictionary<string, ICommand>();
        commands.Add("help", new HelpCommand());
        commands.Add("cls", new ClearCommand());
        commands.Add("connect", new ConnectCommand()); // for when a new client requests connection
        //commands.Add("lookup", new LookupCommand()); // for when a new client requests connection
        commands.Add("damage", new DamageCommand());
        commands.Add("setscore", new SetScoreCommand());
        commands.Add("disconnect", new DisconnectCommand());
        commands.Add("respawn", new RespawnCommand());
    }

    void Start() {
        GenerateTasks();
        CheckScores();
    }

    public async void CheckScores() {
        // UniTask setScoreTask = SetScoreToZero();
        while (score > 0) {
            // update scores
            if (score > thisRunHigh) thisRunHigh = score;
            timeSurvived += Time.deltaTime;
            // update tasks
            for (int i = tasks.Count-1; i >= 0; i--) {
                Task task = tasks[i];
                task.timeSinceSent += Time.deltaTime;
                if (task.timeSinceSent >= task.timeLimit) {
                    // task failed
                    if (task is ConnectPlayerTask) {
                        // refuse connection
                        ConnectPlayerTask connectTask = (ConnectPlayerTask)task;
                        connectingPlayers.Remove(connectTask.targetPlayer);
                    }
                    // remove the task and incur a penalty
                    task.expired = true;
                    score -= (int)task.timeLimit;
                    tasks.Remove(task);
                    ErrorSound();
                }
            }
            await UniTask.Yield();
        }
        
        // await setScoreTask;
        if(thisRunHigh > highestScore) highestScore = thisRunHigh;
        generateTasks = false;

        losePanel.SetActive(true);
        TMP_Text stats = GameObject.Find("LoseStats").GetComponent<TMP_Text>();
        stats.text = $"Highest Player Satisfaction: {highestScore}\nThis Run's High: {thisRunHigh}\nServer Uptime: {(int)timeSurvived}ms";

        TMP_Text[] gameTexts = gamePanel.GetComponentsInChildren<TMP_Text>();
        TMP_Text[] loseTexts = losePanel.GetComponentsInChildren<TMP_Text>();

        foreach(TMP_Text loseText in loseTexts) {
            Color c = loseText.color;
            c.a = 0f;
            loseText.color = c;
        }

        while(gameTexts[0].color.a > 0f) {
            foreach(TMP_Text gameText in gameTexts) {
                Color c = gameText.color;
                c.a -= 0.03f;
                gameText.color = c;
            }

            foreach(TMP_Text loseText in loseTexts) {
                Color c = loseText.color;
                c.a += 0.03f;
                loseText.color = c;
            }

            await UniTask.Delay(10);
        }

        gamePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static int tasksPerDifficultyIncrease = 3;
    public static int difficultyIncreaseAmount = 50;
    public static int tasksAfterDifficultyIncrease = 0;
    public static int difficulty = 0;
    public static int difficultyLimit = 300;
    public static int tasksCompleted = 0;
    
    public async void GenerateTasks() {
        await UniTask.Delay(5000);
        // 0: Connect player
        // 1: Disconect player (Destroy them)
        // 2: Set player score
        // 3: Damage player
        // 4: TODO Player killed -> TODO which then creates change score task
        newTask: int taskType = (int)Random.Range(0, 5);
        switch (taskType) {
            case 0:
                // dont connect too many players
                if (players.Count >= 8) goto newTask;
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
                tasks.Add(new ConnectPlayerTask((int)Random.Range(25, 30), Random.Range(35,40) - difficulty/20, requester));
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
                tasks.Add(new TimeoutPlayerTask(timedOutPlayer, (int)Random.Range(20, 25) - difficulty/30));
                break;
            case 2:
                if (players.Count < 5) goto newTask;
                // get a new player
                Player player = players[(int)Random.Range(0, players.Count - 1)];
                // abort if we already have a task for this player
                foreach (Task task in tasks) {
                    if (task.targetPlayer == player) goto newTask;
                }
                // note: probability of getting here is lower than other tasks, since we need two people
                tasks.Add(new SetScoreTask((int) Random.Range(15, 25), Random.Range(30,40) - difficulty/25, player, player.score + (int)Random.Range(1, 3)));
                break;
            case 3:
                if(players.Count < 3) goto newTask;
                // get a new player
                player = players[(int)Random.Range(0, players.Count - 1)];
                // abort if we already have a task for this player
                foreach (Task task in tasks) {
                    if (task.targetPlayer == player) goto newTask;
                }
                tasks.Add(new DamagePlayerTask((int)Random.Range(15,25), ((byte)Random.Range(30,35) - difficulty/25), player, (int)Random.Range(1,110)));
                break;
            default:
                goto newTask;
        }
        tasksAfterDifficultyIncrease++;

        if(tasksAfterDifficultyIncrease >= tasksPerDifficultyIncrease && difficulty < difficultyLimit) {
            tasksAfterDifficultyIncrease = 0;
            difficulty += difficultyIncreaseAmount;
        }

        AudioManager.PlaySound(STATE.newTask);
        await UniTask.WaitUntil(() => tasks.Count < 7);
        //int delay = Random.Range(0, 2000 - difficulty) + 200 + tasks.Count * taskFrequency - difficulty * 6 / 5;
        int delay = (int)(Random.Range(0, 1000) + difficultyCurve.Evaluate(tasksCompleted) * 800);
        await UniTask.Delay(delay > 0 ? delay : 0);
        if (generateTasks) goto newTask;
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
                score -= (int)tasks[i].timeLimit;
                result += "\nTask Failed Successfully.";
                tasks.RemoveAt(i);
                taskFrequency -= 30;
            }
            else if (tasks[i].IsSatisfied()) {
                tasksCompleted++;
                AudioManager.PlaySound(STATE.winTask);
                score += (int)tasks[i].timeLimit - (int)tasks[i].timeSinceSent;
                tasks.RemoveAt(i);
                taskFrequency -= 30;
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