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
        Clear();
        Reselect();
    }

    private void Update() {
        // dont go past the beginning of your input area, and dont backspace out of it
        if (inputField.caretPosition < lastInputPostition) {
            if (Input.GetKey(KeyCode.Backspace)) inputField.text += ">";
            inputField.caretPosition = lastInputPostition;
        }
    }

    public void TakeInput(string input) {
        if (input[input.Length-1] != '\n') input += '\n'; // ensure the line ends with a linebreak
        string command = "";
        if (input.Length - 1 > lastInputPostition) {
            command = input.Substring(lastInputPostition); // get the part that the player just typed
        }
        string output = GameState.SendCommand(command); // send the command to the gamestate
        if (output != "") inputField.text += output + "\n"; // display the returned string
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
        inputField.caretPosition = inputField.text.Length - 1;
    }

    // usually called by 'cls' command
    public void Clear() {
        inputField.text = ">";
        inputField.caretPosition = 1;
        lastInputPostition = inputField.text.Length;
    }
}