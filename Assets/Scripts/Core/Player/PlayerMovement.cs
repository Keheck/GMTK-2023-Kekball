using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private bool backgroundIsFloor;
    [SerializeField] private bool useMouseToMove;
    [SerializeField] private bool parentCamToPlayer;

    [SerializeField] private float walkSpeed;
    // Is unused when backgroundIsFloor is true
    [SerializeField] private float jumpStrength;

    private CharacterController controller;
    private Vector2 target;

    // Start is called before the first frame update
    void Start() {
        // Get CharacterController or add if it doesn't exist
        this.controller = GetComponent<CharacterController>() ?? this.gameObject.AddComponent<CharacterController>();

        if(this.parentCamToPlayer)
            Camera.main.transform.parent = this.transform;

        if(!useMouseToMove) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update() {
        // We don't want to move just yet
        this.target = this.transform.position;

        if(this.useMouseToMove) {
            if(Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out RaycastHit hit, 1);
                this.target = hit.point;
            }
            
            if(backgroundIsFloor) {
                Vector2 direction = (target - (Vector2)this.transform.position).normalized * walkSpeed * Time.deltaTime;
                this.controller.Move(direction);
            }
        }
    }

    void FixedUpdate() {

    }
}
