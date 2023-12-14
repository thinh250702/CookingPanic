using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrillSlot : ContainerObject, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private GrillStation grillStation;
    [SerializeField] private IngredientObjectSO[] validObjectSOArray;
    [SerializeField] private GameObject[] previewObjectArray;

    public bool isTurnOn;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private IngredientObject fryingObject;

    private void Start() {
        state = State.Idle;
        Player.Instance.OnSelectedObjectChanged += Instance_OnSelectedObjectChanged;
    }

    private void Update() {
        if (HasChildrenObject()) {
            if (isTurnOn) {
                switch (state) {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
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
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                        });
                        if (burningTimer > burningRecipeSO.burningTimerMax) {
                            // Burned
                            fryingObject.DestroySelf();
                            fryingObject = IngredientObject.SpawnKitchenObject(burningRecipeSO.output, this, this.GetObjectFollowTransform());
                            Debug.Log("Object Burned!");
                            state = State.Burned;
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                                progressNormalized = 0f
                            });
                        }
                        break;
                    case State.Burned:
                        break;
                }
            } else {
                // do nothing
            }
            
            Debug.Log(fryingTimer);
        } else {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = 0f
            });
        }
    }

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
