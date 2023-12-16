using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using static GrillSlot;

public class FryBasket : FunctionalSlot {

    [SerializeField] private FryStation fryStation;
    [SerializeField] private IngredientObjectSO rawPotatoSO;

    [SerializeField] private Transform fryingStateTransform;
    [SerializeField] private Transform dryingStateTransform;

    private bool isDownPosition;

    private void Start() {
        isDownPosition = false;
        transform.position = dryingStateTransform.position;

        state = State.Idle;
    }

    private void Update() {
        if (HasChildrenObject()) {
            if (isTurnedOn && isDownPosition) {
                switch (state) {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer += Time.deltaTime;
                        InvokeOnProgressChanged(fryingTimer / fryingRecipeSO.fryingTimerMax);
                        if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                            // Fried
                            fryingObject.DestroySelf();
                            fryingObject = IngredientObject.SpawnKitchenObject(fryingRecipeSO.output, this, this.GetObjectFollowTransform(), Quaternion.Euler(0, 90, 0));
                            Debug.Log("Object Fried!");
                            state = State.Fried;
                            burningTimer = 0f;
                            burningRecipeSO = fryStation.GetBurningRecipeSOWithInput(fryingObject.GetIngredientObjectSO());
                            InvokeOnStateChanged(state);
                        }
                        break;
                    case State.Fried:
                        burningTimer += Time.deltaTime;
                        InvokeOnProgressChanged(burningTimer / burningRecipeSO.burningTimerMax);
                        if (burningTimer > burningRecipeSO.burningTimerMax) {
                            // Burned
                            fryingObject.DestroySelf();
                            fryingObject = IngredientObject.SpawnKitchenObject(burningRecipeSO.output, this, this.GetObjectFollowTransform(), Quaternion.Euler(0, 90, 0));
                            Debug.Log("Object Burned!");
                            state = State.Burned;
                            InvokeOnProgressChanged(0f);
                            InvokeOnStateChanged(state);
                        }
                        break;
                    case State.Burned:
                        break;
                }
            } else {
                // do nothing
            }
        } else {
            state = State.Idle;
            InvokeOnStateChanged(state);
            InvokeOnProgressChanged(0f);
        }
    }

    public IngredientObject GetFryingObject() {
        return this.fryingObject;
    }

    private void TogglePosition() {
        if (isDownPosition) {
            isDownPosition = false;
            StartCoroutine(MoveToSpot(transform, dryingStateTransform.position, Quaternion.identity));
        } else {
            isDownPosition = true;
            StartCoroutine(MoveToSpot(transform, fryingStateTransform.position, Quaternion.identity));
        }
    }

    public override void Interact(Player player) {
        // case: player is holding something
        if (player.HasChildrenObject()) {
            if (player.GetChildrenObject()[0] is IngredientObject) {
                // Player is carrying ingredient - check if it's raw potato or not
                IngredientObject playerIngredient = player.GetChildrenObject()[0] as IngredientObject;
                if (fryStation.HasRecipeWithInput(playerIngredient.GetIngredientObjectSO())) {
                    if (isTurnedOn) {
                        if (!HasChildrenObject()) {
                            if (!isDownPosition) {
                                TogglePosition();
                                Player.Instance.GetChildrenObject()[0].DestroySelf();
                                fryingObject = IngredientObject.SpawnKitchenObject(rawPotatoSO, this, this.GetObjectFollowTransform(), Quaternion.Euler(0, 90, 0));

                                fryingRecipeSO = fryStation.GetFryingRecipeSOWithInput(fryingObject.GetIngredientObjectSO());

                                state = State.Frying;
                                fryingTimer = 0f;

                                InvokeOnStateChanged(state);
                                InvokeOnProgressChanged(0f);
                            }
                        } else {
                            PopupMessageUI.Instance.SetMessage("The fry basket has already had fries!");
                        }
                    } else {
                        PopupMessageUI.Instance.SetMessage("You have to turn on the fry station first!");
                    }
                } else {
                    PopupMessageUI.Instance.SetMessage("Invalid ingredient!");
                }
            } else if (player.GetChildrenObject()[0] is FoodPackage) {
                // Player is carrying food package - check if it's fries carton or not
                FoodPackage foodPackage = player.GetChildrenObject()[0] as FoodPackage;
                if (foodPackage.GetPackageType() == FoodPackage.Type.FriesCarton) {
                    if (HasChildrenObject()) {
                        if (!isDownPosition) {
                            if (foodPackage.TryAddFries(fryingObject)) {
                                fryingObject.DestroySelf();

                                state = State.Idle;
                                InvokeOnStateChanged(state);
                                InvokeOnProgressChanged(0f);
                            } else {
                                PopupMessageUI.Instance.SetMessage("Invalid object!");
                            }
                        } else {
                            TogglePosition();
                        }
                    } else {
                        PopupMessageUI.Instance.SetMessage("The fry basket hasn't had any fries!");
                    }
                    /*if (!isTurnedOn) {
                        
                    } else {
                        PopupMessageUI.Instance.SetMessage("You have to turn off the fry station!");
                    }*/
                } else {
                    PopupMessageUI.Instance.SetMessage("You should pick a fries carton!");
                }
            }
        } 
        // case: player is not holding anything
        else {
            TogglePosition();
        }
    }
}
