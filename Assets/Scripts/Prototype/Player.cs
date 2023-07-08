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

}