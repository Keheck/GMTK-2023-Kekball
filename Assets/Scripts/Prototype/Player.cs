using System.Collections.Generic;

public class Player {
    public int id;
    public string name;
    public int ping;
    public int score;
    public bool timedOut = false;
    public bool isAlive = true;

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
        "bingus_gaming",
        "markbrownianmotion",
        "markblack",
        "markgray",
        "markgrey",
        "markwhite",
        "jesse",
        "the_player",
        "goodmanlaywer",
        "badwoman",
        "alina",
        "mattsports",
        "gaben3",
        "chillen",
        "lucas",
        "blougi",
        "nimkers",
        "toinoing",
        "agent47",
        "jamesmay",
        "fitmc",
        "markmusk",
        "popbob"
    };

    private int Health; //lol
    public int health {
        get { return Health; }
        set {
            this.Health = value;
            if(Health <= 0) {
                isAlive = false;
                GameState.tasks.Add(new RespawnPlayerTask(0, 60, this));
            }
        }
    }
}