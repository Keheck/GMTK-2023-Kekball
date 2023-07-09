using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;

public class ChatPlayer : MonoBehaviour
{
    public TextAsset chat;
    public int delayBetweenChars = 50;
    public int delayBetweenMessages = 1500;
    public int delayBetweenNameAndMessage = 500;
    public int waitToFade = 500;
    public int delayBetweenFade = 10;
    [Range(0.001f, 0.2f)]
    public float fadeAmount = 0.001f;
    public AudioClip typeSound;
    public AudioClip sendSound;

    private TMP_Text tmpText;

    // Start is called before the first frame update
    void Start() {
        this.tmpText = GetComponent<TMP_Text>();
        PlayMessage();
    }

    private void Update() {
        // speed up
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
            delayBetweenChars = (int)(delayBetweenChars * 0.8f);
            delayBetweenMessages = (int)(delayBetweenMessages * 0.8f);
            delayBetweenNameAndMessage = (int)(delayBetweenNameAndMessage * 0.8f);
        }
    }

    async void PlayMessage() {
        string text = chat.text;
        //0 = typing character name
        //1 = typing message
        int state = 0;

        for(int i = 0; i < text.Length; i++) {
            if(state == 0) {
                tmpText.text += text[i];
                if(text[i] == ':') {
                    state = 1;
                    AudioManager.PlaySound(sendSound);
                    await UniTask.Delay(delayBetweenNameAndMessage);
                }
                continue;
            }
            if(state == 1) {
                tmpText.text += text[i];

                if(text[i] == '\n') {
                    state = 0;
                    await UniTask.Delay(delayBetweenMessages);
                    continue;
                }

                AudioManager.PlaySound(typeSound);
                await UniTask.Delay(delayBetweenChars);
            }
        }
        

        await UniTask.Delay(waitToFade);

        while(tmpText.color.a > 0) {
            Color c = tmpText.color;
            c.a -= fadeAmount;
            tmpText.color = c;
            await UniTask.Delay(delayBetweenFade);
        }

        await UniTask.Delay(500);

        SceneManager.LoadScene("PrototypeGameserverGame");
    }
}
