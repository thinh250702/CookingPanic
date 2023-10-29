using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RefrigeratorDoorController : MonoBehaviour
{
    private Animator animator;
    private bool leftDoorOpen = false;
    private bool rightDoorOpen = false;
    [SerializeField] private Door leftDoor;
    [SerializeField] private Door rightDoor;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        leftDoor.OnDoorInteract += LeftDoor_OnDoorInteract;
        rightDoor.OnDoorInteract += RightDoor_OnDoorInteract;
    }

    private void RightDoor_OnDoorInteract(object sender, System.EventArgs e) {
        if (!rightDoorOpen) {
            animator.Play("RRightDoor_Open");
            rightDoorOpen = true;
        } else {
            animator.Play("RRightDoor_Close");
            rightDoorOpen = false;
        }
    }

    private void LeftDoor_OnDoorInteract(object sender, System.EventArgs e) {
        if (!leftDoorOpen) {
            animator.Play("RLeftDoor_Open");
            leftDoorOpen = true;
        } else {
            animator.Play("RLeftDoor_Close");
            leftDoorOpen = false;
        }
    }
}
