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

    public void SwitchNavigation(MenuSection menu) {
        // This is so fucking stupid
        menu.gameObject.SetActive(true);
        GetComponentInParent<MenuSection>().gameObject.SetActive(false);
    }

    public void CloseGame() {
        Application.Quit();
    }
}
