using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class IngredientObjectSO : ScriptableObject, IEquatable<IngredientObjectSO> {
    public string ingredientName;
    public Transform prefab;

    public bool Equals(IngredientObjectSO other) {
        if (this.ingredientName == other.ingredientName) {
            return true;
        } else {
            return false;
        }

    }
}
