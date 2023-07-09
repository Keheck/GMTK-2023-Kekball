using UnityEngine.SceneManagement;

public class QuitCommand : ICommand{
    public string Run(string[] args) {
        SceneManager.LoadScene("MenuScreen");
        return "";
    }

    public string GetUsage() {
        return "exit - quits to the main menu";
    }
}