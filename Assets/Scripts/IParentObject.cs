using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParentObject
{
    public Transform GetObjectFollowTransform();
    public List<PickableObject> GetChildrenObject();
    public void AddChildrenObject(PickableObject pickableObject);
    public void RemoveChildrenObject(PickableObject pickableObject);
    public bool HasChildrenObject();
}
