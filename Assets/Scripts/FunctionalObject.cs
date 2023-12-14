using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalObject : InteractableObject
{
    [SerializeField] protected ContainerObjectSO functionalObjectSO;

    public ContainerObjectSO GetFunctionalObjectSO() {
        return functionalObjectSO;
    }
}
