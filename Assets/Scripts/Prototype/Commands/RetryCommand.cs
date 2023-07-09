using UnityEngine.SceneManagement;

public class RetryCommand : ICommand{
    public string Run(string[] args) {
        SceneManager.LoadScene("PrototypeGameserverGame");
        return "";
    }

    public string GetUsage() {
        return "retry - reboot the server";
    }
}