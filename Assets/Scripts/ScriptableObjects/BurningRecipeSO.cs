using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject {
    public IngredientObjectSO input;
    public IngredientObjectSO output;
    public float burningTimerMax;
}
