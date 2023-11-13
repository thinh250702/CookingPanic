using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject {
    public IngredientObjectSO input;
    public IngredientObjectSO output;
    public int cuttingProgressMax;
}
