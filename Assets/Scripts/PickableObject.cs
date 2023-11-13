using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class PickableObject : InteractableObject {

    protected IParentObject kitchenObjectParent;
    private float moveTime = .2f;

    protected IEnumerator MoveToSpot(Transform objectTransform, Vector3 targetPoint, Quaternion rotation) {
        float elapsedTime = 0;
        float waitTime = moveTime;

        Vector3 start = objectTransform.position;
        Vector3 end = targetPoint;
        Vector3 centerPivot = (start + end) * 0.5f;

        while (elapsedTime < waitTime) {
            float percentageComplete = elapsedTime / waitTime;

            centerPivot -= Vector3.up * .2f;
            Vector3 startRelCenter = start - centerPivot;
            Vector3 endRelCenter = end - centerPivot;

            //objectTransform.position = Vector3.Lerp(objectTransform.position, targetPoint, percentageComplete);
            objectTransform.position = Vector3.Slerp(startRelCenter, endRelCenter, percentageComplete);
            objectTransform.position += centerPivot;

            objectTransform.localRotation = Quaternion.Slerp(objectTransform.localRotation, rotation, percentageComplete);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectTransform.position = targetPoint;
    }

    protected IEnumerator ActivateRigibody() {
        yield return null;
        if (this.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
            rigidbody.isKinematic = false;
        }
    }

    protected IEnumerator DeactivateRigibody() {
        yield return null;
        if (this.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
            rigidbody.isKinematic = true;
        }
    }

    public void SetObjectParent(IParentObject parentObject) {
        IParentObject previousParent = kitchenObjectParent;
        if (previousParent != null) {
            previousParent.RemoveChildrenObject(this);
        }
        transform.SetParent(parentObject.GetObjectFollowTransform());
        parentObject.AddChildrenObject(this);
        kitchenObjectParent = parentObject;
    }

    public void NormalDropObject(IParentObject parentObject, Vector3 dropPosition, Quaternion rotation) {
        StartCoroutine(MoveToSpot(this.transform, dropPosition, rotation));
        this.SetObjectParent(parentObject);
        //Enable Rigidbody to make the object falling
        StartCoroutine(ActivateRigibody());
    }

    public void DropConcaveContainer(IParentObject parentObject, Vector3 dropPosition, Quaternion rotation) {
        StartCoroutine(MoveToSpot(this.transform, dropPosition, rotation));
        this.SetObjectParent(parentObject);
        if (parentObject is Trashbin) {
            this.GetComponent<MeshCollider>().convex = true;
            StartCoroutine(ActivateRigibody());
        } else {
            StartCoroutine(DeactivateRigibody());
        }
    }

    public void NormalPickObject(Player player, Quaternion rotation) {
        StartCoroutine(MoveToSpot(this.transform, player.GetObjectFollowTransform().position, rotation));
        this.SetObjectParent(player);
        //Disable Rigidbody to prevent the object from falling
        StartCoroutine(DeactivateRigibody());
    }

    public void DestroySelf() {
        kitchenObjectParent.RemoveChildrenObject(this);
        Destroy(gameObject);
    }

    public bool TryGetFoodContainer(out FoodContainer foodContainer) {
        if (this is FoodContainer) {
            foodContainer = this as FoodContainer;
            return true;
        } else {
            foodContainer = null;
            return false;
        }
    }
}
    
    