using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrillSlot : FunctionalSlot {

    [SerializeField] private GrillStation grillStation;
    [SerializeField] private IngredientObjectSO[] validObjectSOArray;
    [SerializeField] private GameObject[] previewObjectArray;

    private void Start() {
        Player.Instance.OnSelectedObjectChanged += Instance_OnSelectedObjectChanged;
    }

    private void Update() {
        if (HasChildrenObject()) {
            if (isTurnedOn) {
                switch (state) {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer += Time.deltaTime;
                        InvokeOnProgressChanged(fryingTimer / fryingRecipeSO.fryingTimerMax);
                        if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                            // Fried
                            fryingObject.DestroySelf();
                            fryingObject = IngredientObject.SpawnKitchenObject(fryingRecipeSO.output, this, this.GetObjectFollowTransform(), Quaternion.identity);
                            state = State.Fried;
                            burningTimer = 0f;
                            burningRecipeSO = grillStation.GetBurningRecipeSOWithInput(fryingObject.GetIngredientObjectSO());
                            InvokeOnStateChanged(state);
                        }
                        break;
                    case State.Fried:
                        burningTimer += Time.deltaTime;
                        InvokeOnProgressChanged(burningTimer / burningRecipeSO.burningTimerMax);
                        if (burningTimer > burningRecipeSO.burningTimerMax) {
                            // Burned
                            fryingObject.DestroySelf();
                            fryingObject = IngredientObject.SpawnKitchenObject(burningRecipeSO.output, this, this.GetObjectFollowTransform(), Quaternion.identity);
                            state = State.Burned;
                            InvokeOnProgressChanged(0f);
                            InvokeOnStateChanged(state);
                        }
                        break;
                    case State.Burned:
                        break;
                }
            }
        } else {
            state = State.Idle;
            InvokeOnStateChanged(state);
            InvokeOnProgressChanged(0f);
        }
    }

    // Handle preview object visual
    private void Instance_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e) {
        if (e.selectedObject == this) {
            if (!HasChildrenObject() && Player.Instance.HasChildrenObject() && Player.Instance.GetChildrenObject()[0] is IngredientObject) {
                IngredientObject playerIngredient = Player.Instance.GetChildrenObject()[0] as IngredientObject;
                if (validObjectSOArray.Contains(playerIngredient.GetIngredientObjectSO())) {
                    if (playerIngredient.GetIngredientObjectSO() == validObjectSOArray[0]) {
                        previewObjectArray[0].SetActive(true);
                    } else {
                        previewObjectArray[1].SetActive(true);
                    }
                }
            }
        } else {
            foreach (GameObject gameObject in previewObjectArray) {
                gameObject.SetActive(false);
            }
        }
    }

    public IngredientObject GetFryingObject() {
        return this.fryingObject;
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject() && player.GetChildrenObject()[0] is IngredientObject) {
            // Player is carrying ingredient
            IngredientObject playerIngredient = player.GetChildrenObject()[0] as IngredientObject;
            if (grillStation.HasRecipeWithInput(playerIngredient.GetIngredientObjectSO())) {
                if (isTurnedOn) {
                    fryingObject = playerIngredient;
                    playerIngredient.NormalDropObject(this, this.GetObjectFollowTransform().position, Quaternion.identity);
                    fryingRecipeSO = grillStation.GetFryingRecipeSOWithInput(fryingObject.GetIngredientObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    InvokeOnProgressChanged(0f);
                    InvokeOnStateChanged(state);
                } else {
                    PopupMessageUI.Instance.SetMessage("You have to turn on the grill station first!");
                }
            } else {
                PopupMessageUI.Instance.SetMessage("Object can't be fried!");
            }
        }
    }
}
