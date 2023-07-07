using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class Terminal : MonoBehaviour {

    TMP_InputField inputField;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        inputField = GetComponent<TMP_InputField>();
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void TakeInput(string input) {
        
    }

    // called when deselected by user, to force you back in
    public async void Reselect() {
        await UniTask.Yield();
        inputField.Select();
    }
}