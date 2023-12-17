using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockObject : PickableObject {

    [SerializeField] private StockObjectSO stockObjectSO;
    [SerializeField] private Transform originalTransform;

    //private int currentQuantity;
    public int CurrentQuantity { get; set; }

    public event EventHandler<OnQuantityChangedEventArgs> OnQuantityChanged;
    public class OnQuantityChangedEventArgs : EventArgs {
        public int quantity;
    }

    private void Start() {
        if (stockObjectSO != null) {
            CurrentQuantity = stockObjectSO.quantity;
        }
    }

    public StockObjectSO GetStockObjectSO() {
        return this.stockObjectSO;
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            // Player is holding something
            if (player.GetChildrenObject()[0] is ContainerObject) {
                // Player is holding a container
                ContainerObject container = player.GetChildrenObject()[0] as ContainerObject;
                if (container is RaycastDropContainer) {
                    Transform containerTransform = container.GetObjectFollowTransform();
                    Vector3 dropPoint = new Vector3(containerTransform.position.x, containerTransform.position.y + .2f, containerTransform.position.z);
                    PickableObject stockObject = InstantiateStockObject();
                    stockObject.NormalDropObject(container, dropPoint, Quaternion.identity);
                    CurrentQuantity--;
                    // Phat di su kien
                    OnQuantityChanged?.Invoke(this, new OnQuantityChangedEventArgs {
                        quantity = CurrentQuantity
                    });
                } 
            } else {
                // Player is holding an ingredient - do nothing
            }
        } else {
            // Player is not carrying anything
            PickableObject stockObject = InstantiateStockObject();
            stockObject.NormalPickObject(player, Quaternion.identity);
            CurrentQuantity--;
            // Phat di su kien
            OnQuantityChanged?.Invoke(this, new OnQuantityChangedEventArgs {
                quantity = CurrentQuantity
            });
        }
    }

    public void ResetQuantity() {
        CurrentQuantity = stockObjectSO.quantity;
    }

    private PickableObject InstantiateStockObject() {
        Transform stockObjectTransform = Instantiate(stockObjectSO.prefab, originalTransform.position, Quaternion.identity);
        return stockObjectTransform.GetComponent<PickableObject>();
    }
}
