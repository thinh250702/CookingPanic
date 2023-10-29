using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingBoard : ContainerObject {

    public event EventHandler<EventArgs> OnCut;

    [SerializeField] private Transform playerHoldPoint;

    private Vector3 originalPosition;
    private Vector3 playerHitPoint;
    private bool isEnter;

    private void Start() {
        originalPosition = playerHoldPoint.localPosition;
        holdingObjectList = new List<PickableObject>();
    }

    private void Update() {
        RaycastHit playerRaycastHit = Player.Instance.GetPlayerRaycastHit();
        if (playerRaycastHit.transform != null) {
            if (playerRaycastHit.transform.TryGetComponent<CuttingBoard>(out CuttingBoard cuttingBoard)) {
                playerHitPoint = playerRaycastHit.point;
                if (isEnter) {
                    playerHoldPoint.position = new Vector3(playerHitPoint.x, playerHitPoint.y + .2f, playerHitPoint.z);
                    //StartCoroutine(MoveToSpot(playerHoldPoint, new Vector3(playerHitPoint.x, playerHitPoint.y + .2f, playerHitPoint.z), Quaternion.identity));
                    //Debug.Log(playerHoldPoint.localPosition);
                } else {
                    playerHoldPoint.localPosition = originalPosition;
                }
            } else {
                playerHoldPoint.localPosition = originalPosition;
            }
        }
    }
    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            // Player is carrying something
            if (player.GetChildrenObject()[0] is CuttingKnife) {
                // Player is carrying a knife
                Debug.Log("Player is carrying a knife");
                if (!isEnter) {
                    isEnter = true;
                } else {
                    isEnter = false;
                }
            }
            if (player.GetChildrenObject()[0] is IngredientObject) {
                Debug.Log("Player is carrying an IngredientObject");
                IngredientObject playerIngredient = player.GetChildrenObject()[0] as IngredientObject;
                NormalDropObject(playerIngredient, this, this.GetObjectFollowTransform().position);
            }
        } else {
            // Player is not carrying anything
        }
    }

    public override void InteractAlternate(Player player) {
        if (isEnter) {
            OnCut?.Invoke(this, EventArgs.Empty);
        }
    }
}
