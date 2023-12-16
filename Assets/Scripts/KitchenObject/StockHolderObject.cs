using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockHolderObject : ContainerObject, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private StockObject stockObject;
    [SerializeField] private bool requirePreparing;
    [SerializeField] private IngredientObjectSO validObjectSO;

    public enum State {
        Empty,
        Filling,
        Filled,
    }

    private State state;
    private float fillingTimer;
    private float fillingTimeMax = 5f;

    private void Start() {
        stockObject.OnQuantityChanged += StockObject_OnQuantityChanged;
        state = State.Filled;
    }

    private void Update() {
        if (!requirePreparing) {
            switch (state) {
                case State.Empty:
                    break;
                case State.Filling:
                    fillingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fillingTimer / fillingTimeMax
                    });
                    if (fillingTimer > fillingTimeMax) {
                        // Filled
                        PopupMessageUI.Instance.SetMessage("The ingredient has been restocked!");
                        stockObject.gameObject.SetActive(true);
                        state = State.Filled;
                        stockObject.ResetQuantity();
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Filled:
                    break;
            }
        }
    }

    private void StockObject_OnQuantityChanged(object sender, StockObject.OnQuantityChangedEventArgs e) {
        if (e.quantity == 0) {
            stockObject.gameObject.SetActive(false);
            state = State.Empty;
        } else {
            stockObject.gameObject.SetActive(true);
        }
    }

    public override void Interact(Player player) {
        if (requirePreparing && validObjectSO != null) {
            if (stockObject.CurrentQuantity == 0 && player.HasChildrenObject() && player.GetChildrenObject()[0] is IngredientObject) {
                IngredientObject playerHoldingObject = player.GetChildrenObject()[0] as IngredientObject;
                if (playerHoldingObject.GetIngredientObjectSO() == validObjectSO) {
                    playerHoldingObject.NormalDropObject(this, this.GetObjectFollowTransform().position, Quaternion.identity);
                    StartCoroutine(DestroyChildrenObject(.2f));
                    stockObject.gameObject.SetActive(true);
                    stockObject.ResetQuantity();
                }
            }
        } else {
            if (state == State.Empty) {
                if (GameHandler.Instance.UpdateExpenses(stockObject.GetStockObjectSO().restockPrice)) {
                    state = State.Filling;
                    fillingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fillingTimer / fillingTimeMax
                    });
                }
            } else {
                // do nothing
            }
        }
    }

    public StockObject GetStockObject() {
        return this.stockObject;
    }
}
