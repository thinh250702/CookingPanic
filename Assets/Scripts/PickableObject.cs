using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : InteractableObject {

    [SerializeField] protected KitchenObjectSO kitchenObjectSO;

    protected IParentObject kitchenObjectParent;
    private float moveTime = .2f;

    protected IEnumerator MoveToSpot(Transform objectTransform, Vector3 targetPoint, Quaternion rotation) {
        float elapsedTime = 0;
        float waitTime = moveTime;

        while (elapsedTime < waitTime) {
            float percentageComplete = elapsedTime / waitTime;
            objectTransform.position = Vector3.Lerp(objectTransform.position, targetPoint, percentageComplete);
            objectTransform.localRotation = Quaternion.Slerp(objectTransform.localRotation, rotation, percentageComplete);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectTransform.position = targetPoint;
    }

    protected IEnumerator ActivateRigibody(PickableObject pickableObject) {
        yield return null;
        if (pickableObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
            rigidbody.isKinematic = false;
        }
    }

    protected IEnumerator DeactivateRigibody(PickableObject pickableObject) {
        yield return null;
        if (pickableObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
            rigidbody.isKinematic = true;
        }
    }

    protected void SetObjectParent(PickableObject pickableObject, IParentObject parentObject) {
        IParentObject previousParent = pickableObject.kitchenObjectParent;
        if (previousParent != null) {
            previousParent.RemoveChildrenObject(pickableObject);
        }
        pickableObject.transform.SetParent(parentObject.GetObjectFollowTransform());
        parentObject.AddChildrenObject(pickableObject);
        pickableObject.kitchenObjectParent = parentObject;
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IParentObject parentObject, Vector3 spawnPosition) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, spawnPosition, Quaternion.identity);
        PickableObject kitchenObject = kitchenObjectTransform.GetComponent<PickableObject>();
        kitchenObject.SetObjectParent(kitchenObject, parentObject);
        if (parentObject is Player) {
            kitchenObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    protected void NormalDropObject(PickableObject playerHoldingObject, IParentObject parentObject, Vector3 dropPosition) {
        StartCoroutine(MoveToSpot(playerHoldingObject.transform, dropPosition, Quaternion.identity));
        StartCoroutine(ActivateRigibody(playerHoldingObject));
        SetObjectParent(playerHoldingObject, parentObject);
    }

    protected void NormalPickObject(PickableObject pickObject, Player player, Quaternion rotation) {
        StartCoroutine(MoveToSpot(pickObject.transform, player.GetObjectFollowTransform().position, rotation));
        SetObjectParent(pickObject, player);
        //Disable Rigidbody to prevent the object from falling
        StartCoroutine(DeactivateRigibody(pickObject));
    }
}
    
    