using UnityEngine;
using System.Collections.Generic;

public class Player {
    public int id;
    public string name;
    public int ping;
    public bool timedOut = false;
    public bool isAlive = true;

    public event System.Action<int> OnDamaged;
    public event System.Action<int> OnScoreChanged;

    public Player(int id, string name, int ping) {
        this.id = id;
        this.name = name;
        this.ping = ping;
        this.health = 100;
        this.score = 0;
    }

    public static readonly List<string> PLAYER_NAMES = new List<string>() {
        "keheck",
        "doeball",
        "zachary",
        "joemama",
        "bingusgaming",
        "markbrownianmotion",
        "markgray",
        "markgrey",
        "jesse",
        "theplayer",
        "goodmanlawyer",
        "badwoman",
        "alina",
        "mattsports",
        "gaben3",
        "chillen",
        "lucas",
        "blougi",
        // "connect",
        // "help",
        // "disconnect",
        "nimkers",
        "toinoing",
        "agent47",
        "jamesmay",
        "fitmc",
        "markmusk",
        "facebookman",
        "sneaks",
        "wirtual",
        "popbob"
    };

    private int Health; //lol
    public int health {
        get { return Health; }
        set {
            OnDamaged?.Invoke(this.Health - value);
            this.Health = value;
            if(Health <= 0) {
                isAlive = false;
                GameState.tasks.Add(new RespawnPlayerTask(0, 60, this));
            }
        }
    }
    
    private int Score; //lol
    public int score {
        get { return Score; }
        set {
            OnScoreChanged?.Invoke(value - this.Score);
            this.Score = value;
        }
    }
}