using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class QuitCommand : ICommand{
    
    public string Run(string[] args) {
        Quit();
        return "";
    }

    public string GetUsage() {
        return "exit - quits to the main menu";
    }

    async void Quit() {
        await UniTask.Delay(200);
        SceneManager.LoadScene("MenuScreen");
    }
}