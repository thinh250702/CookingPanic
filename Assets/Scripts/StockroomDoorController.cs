using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockroomDoorController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    [SerializeField] private Door stockroomDoor;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        stockroomDoor.OnDoorInteract += StockroomDoor_OnDoorInteract;
    }

    private void StockroomDoor_OnDoorInteract(object sender, System.EventArgs e) {
        if (!isOpen) {
            animator.Play("StockroomDoor_Open");
            isOpen = true;
        } else {
            animator.Play("StockroomDoor_Close");
            isOpen = false;
        }
    }
}
