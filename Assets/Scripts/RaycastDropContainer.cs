using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDropContainer : ContainerObject {

    [SerializeField] private Transform testTransform;

    private Vector3 playerHitPoint;

    private void Start() {
        holdingObjectList = new List<PickableObject>();
    }

    private void Update() {
        RaycastHit playerRaycastHit = Player.Instance.GetPlayerRaycastHit();
        if (playerRaycastHit.transform != null) {
            if (playerRaycastHit.transform.TryGetComponent<RaycastDropContainer>(out RaycastDropContainer container)) {
                playerHitPoint = playerRaycastHit.point;
                //testTransform.position = playerHitPoint;
                StartCoroutine(MoveToSpot(testTransform, playerHitPoint, Quaternion.identity));
            }
        }
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            // Player is carrying something
            Debug.Log("Player is carrying something");
            PickableObject playerHoldingObject = player.GetChildrenObject()[0];
            RaycastHit playerRaycastHit = player.GetPlayerRaycastHit();
            Vector3 dropPosition = new Vector3(playerRaycastHit.point.x, playerRaycastHit.point.y, playerRaycastHit.point.z);
            NormalDropObject(playerHoldingObject, this, dropPosition);

        } else {
            // Player is not carrying anything - pickup that container
            if (canPick) {
                Debug.Log("Doing Pickup");
                NormalPickObject(this, player, Quaternion.identity);
            }
        }
    }
}
