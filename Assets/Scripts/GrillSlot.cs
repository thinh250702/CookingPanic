using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillSlot : ContainerObject {
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private GrillStation grillStation;
    public bool isTurnOn;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private IngredientObject fryingObject;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasChildrenObject()) {
            if (isTurnOn) {
                switch (state) {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer += Time.deltaTime;
                        if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                            // Fried
                            fryingObject.DestroySelf();
                            fryingObject = IngredientObject.SpawnKitchenObject(fryingRecipeSO.output, this, this.GetObjectFollowTransform());
                            Debug.Log("Object Fried!");
                            state = State.Fried;
                            burningTimer = 0f;
                            burningRecipeSO = grillStation.GetBurningRecipeSOWithInput(fryingObject.GetIngredientObjectSO());
                        }
                        break;
                    case State.Fried:
                        burningTimer += Time.deltaTime;
                        if (burningTimer > burningRecipeSO.burningTimerMax) {
                            // Burned
                            fryingObject.DestroySelf();
                            fryingObject = IngredientObject.SpawnKitchenObject(burningRecipeSO.output, this, this.GetObjectFollowTransform());
                            Debug.Log("Object Burned!");
                            state = State.Burned;
                        }
                        break;
                    case State.Burned:
                        break;
                }
            } else {
                // do nothing
            }
            
            Debug.Log(fryingTimer);
        }
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject() && player.GetChildrenObject()[0] is IngredientObject) {
            // Player is carrying ingredient
            IngredientObject playerIngredient = player.GetChildrenObject()[0] as IngredientObject;
            if (grillStation.HasRecipeWithInput(playerIngredient.GetIngredientObjectSO())) {
                if (isTurnOn) {
                    fryingObject = playerIngredient;
                    playerIngredient.NormalDropObject(this, this.GetObjectFollowTransform().position, Quaternion.identity);
                    fryingRecipeSO = grillStation.GetFryingRecipeSOWithInput(fryingObject.GetIngredientObjectSO());
                    state = State.Frying;
                    fryingTimer = 0f;
                } else {
                    Debug.Log("You have to turn on the grill first!");
                }
            } else {
                Debug.Log("Object can't be fried!");
            }
        } else {
            // Player is not carrying anything
        }
    }
}
