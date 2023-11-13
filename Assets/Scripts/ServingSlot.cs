using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class ServingSlot : ContainerObject {
    public Transform customerStartPoint;
    public Transform customerEndPoint;

    //public Transform[] movePointArray;
    // [0], [1]: left or right StartPoint
    // [2]: TurnPoint
    // [3]: EndPoint

    public bool isEngage;
    public CustomerSO currentCustomerSO;
    private CustomerListSO customerListSO;

    //private float waitingTimer;
    //private bool isArrived;

    public Customer CurrentCustomer { get; set; }

    private void Start() {
        customerListSO = ServingManager.Instance.GetCustomerListSO();   
    }

    private void Update() {
        if (!isEngage) {
            // There is no customer at the moment -> Spawn customer
            if (CurrentCustomer == null) {
                // Create customer randomly
                this.currentCustomerSO = customerListSO.customerSOList[UnityEngine.Random.Range(0, customerListSO.customerSOList.Count)];

                CurrentCustomer = InstantiateCustomer(); // Instantiate that customer
                StartCoroutine(CustomerArriving(CurrentCustomer)); // Move from start point to end point
                CurrentCustomer.CurrentSlot = this; // Set that customer CurrentSlot to this slot
                CurrentCustomer.OnCustomerLeaving += CurrentCustomer_OnCustomerLeaving; // That customer subscribe to the leaving event

                isEngage = true;
            }
        } else {
            // There is one customer at the moment
        }
    }

    private IEnumerator CustomerLeaving(Customer customer) {
        yield return customer.MoveCustomer(customerEndPoint.position, customerStartPoint.position, Quaternion.identity);

        Destroy(customer.gameObject); // After the customer had moved to end point -> destroy that customer
        ClearCustomer(); // Set the CurrentCustomer of this slot to null

        yield return new WaitForSeconds(2f); // Wait for 2 seconds before spawn new customer
        isEngage = false; // Ready to spawn new customer
    }

    private IEnumerator CustomerArriving(Customer customer) {
        yield return customer.MoveCustomer(customerStartPoint.position, customerEndPoint.position, Quaternion.Euler(0, 180, 0));
        customer.isArrived = true;
    }

    private void CurrentCustomer_OnCustomerLeaving(object sender, EventArgs e) {
        StartCoroutine(CustomerLeaving(CurrentCustomer));
    }

    public bool ServeOrder(ContainerObject servingTray, List<RecipeSO> customerOrder) {
        if (servingTray.GetChildrenObject().Count == customerOrder.Count) {
            Debug.Log("Have the same number of item");
            // The number of item in the tray equal the number of order item
            bool trayItemsMatchesRecipe = true;
            foreach (PickableObject item in servingTray.GetChildrenObject()) {
                // Loop through each item in the tray
                if (item.TryGetFoodContainer(out FoodContainer foodContainer)) {
                    Debug.Log("Item in the tray is type FoodContainer");
                    // The item is type FoodContainer
                    foreach (RecipeSO orderItem in customerOrder) {
                        // Loop through each item in order
                        IEnumerable<IngredientObjectSO> result = orderItem.componentList.Except(foodContainer.GetIngredientObjectSOList());
                        // Compare two list of IngredientObjectSO
                        if (result.Any()) {
                            // There are some differences between 2 list
                            Debug.Log("Item not matched");
                            trayItemsMatchesRecipe = false;
                            break;
                        } else {
                            // There are no difference between 2 list
                            Debug.Log("Item matched");
                        }
                    }
                } else {
                    trayItemsMatchesRecipe = false;
                    break;
                }
            }
            if (trayItemsMatchesRecipe) {
                return true;
            } else {
                return false;
            }
        }
        return false;
    }

    public void ClearCustomer() {
        CurrentCustomer = null;
    }

    private Customer InstantiateCustomer() {
        Transform customerTransform = Instantiate(currentCustomerSO.prefab, customerStartPoint.position, Quaternion.identity);
        return customerTransform.GetComponent<Customer>();
    }

}
