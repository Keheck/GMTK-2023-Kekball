using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class RetryCommand : ICommand{
    
    public string Run(string[] args) {
        Retry();
        return "";
    }

    public string GetUsage() {
        return "retry - reboot the server";
    }

    async void Retry() {
        await UniTask.Delay(200);
        SceneManager.LoadScene("PrototypeGameserverGame");
    }
}