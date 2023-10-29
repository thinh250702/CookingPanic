using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : PickableObject, IParentObject {

    [SerializeField] protected Transform parentObjectTransform;
    [SerializeField] protected bool canPick;

    protected List<PickableObject> holdingObjectList;

    public void AddChildrenObject(PickableObject pickableObject) {
        holdingObjectList.Add(pickableObject);
    }

    public List<PickableObject> GetChildrenObject() {
        return holdingObjectList;
    }

    public Transform GetObjectFollowTransform() {
        return parentObjectTransform;
    }

    public bool HasChildrenObject() {
        return holdingObjectList.Count != 0;
    }

    public void RemoveChildrenObject(PickableObject pickableObject) {
        foreach (var item in holdingObjectList.ToArray()) {
            if (item == pickableObject) {
                holdingObjectList.Remove(item);
            }
        }
    }

    public void SetChildrenObject(PickableObject pickableObject) {
        holdingObjectList.Add(pickableObject);
    }
}
