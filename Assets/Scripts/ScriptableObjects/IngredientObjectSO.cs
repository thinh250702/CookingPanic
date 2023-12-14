using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class IngredientObjectSO : ScriptableObject, IEquatable<IngredientObjectSO> {
    public string ingredientName;
    public Transform prefab;
    public Sprite sprite;
    public string description = "This is for description";

    public bool Equals(IngredientObjectSO other) {
        if (this.ingredientName == other.ingredientName) {
            return true;
        } else {
            return false;
        }

    }
}
