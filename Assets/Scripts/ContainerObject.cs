using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : PickableObject, IParentObject {

    [SerializeField] protected Transform parentObjectTransform;
    [SerializeField] protected ContainerObjectSO containerObjectSO;

    protected List<PickableObject> holdingObjectList = new List<PickableObject>();

    private void Start() {
        if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) rigidbody.isKinematic = true;
    }

    public ContainerObjectSO GetContainerObjectSO() {
        return containerObjectSO;
    }

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

    public void ClearAllChildrenObject() {
        holdingObjectList.Clear();
    }

    protected IEnumerator DestroyChildrenObject(float second) {
        yield return new WaitForSeconds(second);
        for (int i = 0; i< holdingObjectList.Count; i++) {
            holdingObjectList[i].DestroySelf();
        }
    }
}
