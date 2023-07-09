using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ButtonExtensions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private TMP_Text tmpTextField;
    public AudioClip hover;
    public AudioClip click;

    private string text;

    public void OnPointerEnter(PointerEventData eventData) {
        this.tmpTextField.text = "> " + this.tmpTextField.text;
        AudioManager.PlaySound(this.hover);
    }

    public void OnPointerExit(PointerEventData eventData) {
        this.tmpTextField.text = text;
    }

    void OnEnable() {
        this.tmpTextField.text = this.text;
    }

    // Start is called before the first frame update
    void Awake() {
        tmpTextField = GetComponentInChildren<TMP_Text>();
        this.text = this.tmpTextField.text;
    }

    private void Start() {
        GetComponent<Button>().onClick.AddListener(() => AudioManager.PlaySound(click));
    }

    public void StartGame() {
        SceneManager.LoadScene("Lore");
    }

    // Reset the entire scene manually and set the GameState's variables to default. Super scuffed but it works
    public void RetryGame() {
        GameState instance = GameState.STATE;
        
        TMP_Text[] loseTexts = instance.losePanel.GetComponentsInChildren<TMP_Text>();

        foreach(TMP_Text loseText in loseTexts) {
            Color c = loseText.color;
            c.a = 0f;
            loseText.color = c;
        }
        instance.gamePanel.SetActive(true);
        instance.losePanel.SetActive(false);
        GameState.ClearUserTerminal();
        GameState.connectingPlayers.Clear();
        GameState.players.Clear();
        GameState.tasks.Clear();
        GameState.generateTasks = true;
        // Don't reset score
        //GameState.highestScore = 100;
        GameState.thisRunHigh = 100;
        GameState.score = 100;
        GameState.timeSurvived = 0;
        GameState.difficulty = 0;

        TMP_Text[] gameTexts = instance.gamePanel.GetComponentsInChildren<TMP_Text>();

        foreach(TMP_Text gameText in gameTexts) {
            Color c = gameText.color;
            c.a = 1f;
            gameText.color = c;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        instance.CheckScores();
        instance.GenerateTasks();
        GameObject.Find("Terminal").GetComponent<Terminal>().Reselect();
    }

    public void SwitchNavigation(MenuSection menu) {
        // This is so fucking stupid
        menu.gameObject.SetActive(true);
        GetComponentInParent<MenuSection>().gameObject.SetActive(false);
    }

    public void CloseGame() {
        Application.Quit();
    }
}
