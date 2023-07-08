using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonExtensions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text tmpTextField;

    public AudioClip hover;
    public AudioClip click;

    public void OnPointerEnter(PointerEventData eventData) {
        this.tmpTextField.text = "> " + this.tmpTextField.text;
        AudioManager.PlaySound(this.hover);
    }

    public void OnPointerExit(PointerEventData eventData) {
        string text = this.tmpTextField.text;
        if(text.StartsWith("> "))
            this.tmpTextField.text = text.Substring(2);
    }

    // Start is called before the first frame update
    void Start() {
        tmpTextField = GetComponentInChildren<TMP_Text>();
    }
}
