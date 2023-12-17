using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CuttingBoard : ContainerObject, IHasProgress {

    public static event EventHandler OnAnyCut;
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private Transform playerHoldPoint;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;

    private Vector3 originalPosition;
    private Vector3 playerHitPoint;
    private IngredientObject cuttingObject;

    private bool isEnter;

    private void Start() {
        originalPosition = playerHoldPoint.localPosition;
    }

    private void Update() {
        if (cuttingObject != null) {
            if (isEnter) {
                cuttingObject.GetComponent<BoxCollider>().enabled = false;
                cuttingObject.GetComponent<Rigidbody>().isKinematic = true;
                RaycastHit playerRaycastHit = Player.Instance.GetPlayerRaycastHit();
                if (playerRaycastHit.transform != null && playerRaycastHit.transform.GetComponent<CuttingBoard>()) {
                    playerHitPoint = playerRaycastHit.point;
                    playerHoldPoint.position = new Vector3(playerHitPoint.x, playerHitPoint.y + .2f, playerHitPoint.z);
                } else {
                    playerHoldPoint.localPosition = originalPosition;
                }
            } else {
                cuttingObject.GetComponent<BoxCollider>().enabled = true;
                cuttingObject.GetComponent<Rigidbody>().isKinematic = false;
                playerHoldPoint.localPosition = originalPosition;
            }
        }
    }
    public override void Interact(Player player) {
        if (!HasChildrenObject()) {
            //There is no KitchenObject here
            if (player.HasChildrenObject()) {
                // Player is holding something
                if (player.GetChildrenObject()[0] is IngredientObject) {
                    IngredientObject playerIngredient = player.GetChildrenObject()[0] as IngredientObject;
                    if (HasRecipeWithInput(playerIngredient.GetIngredientObjectSO())) {
                        // Ingredient can be cut
                        PopupMessageUI.Instance.SetMessage("Pick a knife!");
                        playerIngredient.NormalDropObject(this, this.GetObjectFollowTransform().position, Quaternion.identity);
                        cuttingProgress = 0;
                        cuttingObject = playerIngredient;
                        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(cuttingObject.GetIngredientObjectSO());
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                        });
                    } else {
                        PopupMessageUI.Instance.SetMessage("Can't cut this ingredient!");
                    }
                }
                else {
                    // Player is holding something else - do nothing
                }
                
            } else {
                // Player is not holding anything - do nothing
            }
        } else {
            //There is a KitchenObject here
            if (player.GetChildrenObject()[0] is CuttingKnife) {
                if (!isEnter) {
                    PopupMessageUI.Instance.SetMessage("Press E to cut!");
                    isEnter = true;
                } else {
                    isEnter = false;
                }
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (isEnter) {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(cuttingObject.GetIngredientObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                IngredientObjectSO outputObjectSO = GetOutputForInput(cuttingObject.GetIngredientObjectSO());
                cuttingObject.DestroySelf();
                IngredientObject.SpawnKitchenObject(outputObjectSO, this, this.GetObjectFollowTransform(), Quaternion.identity);
                isEnter = false;
                playerHoldPoint.localPosition = originalPosition;
            }
        }
    }

    private bool HasRecipeWithInput(IngredientObjectSO inputObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputObjectSO);
        return cuttingRecipeSO != null;
    }

    private IngredientObjectSO GetOutputForInput(IngredientObjectSO inputObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(IngredientObjectSO inputObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }

    public IngredientObject GetCuttingObject() { 
        return cuttingObject;
    }
}
