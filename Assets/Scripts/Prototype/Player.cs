using System.Collections.Generic;

public class Player {
    public int id;
    public string name;
    public int health;
    public int ping;
    public int score;

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
        "gmtk_mark",
        "the_player",
        "alina",
        "matt",
        "gaben",
        "chillen",
        "lucas",
        "blougi",
        "nimkers",
        "toinoing",
        "fitmc",
        "markmusk",
        "popbob"
    };
}