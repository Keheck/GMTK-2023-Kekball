using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class Terminal : MonoBehaviour {

    TMP_InputField inputField;
    int lastInputPostition = 0;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        inputField = GetComponent<TMP_InputField>();
        lastInputPostition = inputField.text.Length;
        Reselect();
    }

    public void TakeInput(string input) {
        if (input[input.Length-1] != '\n') input += '\n'; // ensure the line ends with a linebreak
        string command = "";
        if (input.Length - 1 > lastInputPostition) {
            command = input.Substring(lastInputPostition);
        }
        // TODO: pass in the command to the task manager or something to get a return value
        print(command);
        // TODO: add return from the players command, then another linebreak
        inputField.text += ">";
        lastInputPostition = inputField.text.Length + 1;
        Reselect();
    }

    // called when deselected by user, to force you back in
    // also called when you take input, since it automatically deselects
    public async void Reselect() {
        await UniTask.Yield();
        inputField.Select();
        inputField.ActivateInputField();
        inputField.MoveTextEnd(false);
    }
}