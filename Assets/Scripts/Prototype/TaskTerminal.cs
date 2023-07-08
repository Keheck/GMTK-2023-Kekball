using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskTerminal : MonoBehaviour {

    TMP_Text textField;

    void Start() {
        textField = GetComponent<TMP_Text>();
    }

    void Update() {
        textField.text = $"Player Satisfaction: {GameState.score}\nTasks:";

        foreach(Task task in GameState.tasks) {
            textField.text += $"\n* {task.GetDescription()} {(int)task.timeSinceSent}/{(int)task.timeLimit}ms";
        }
    }
}
