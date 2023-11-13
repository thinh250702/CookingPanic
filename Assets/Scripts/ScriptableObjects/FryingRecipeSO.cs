using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject {
    public IngredientObjectSO input;
    public IngredientObjectSO output;
    public float fryingTimerMax;
}
