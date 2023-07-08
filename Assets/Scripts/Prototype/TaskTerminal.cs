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
        textField.text = $"{Time.time}";
    }
}
