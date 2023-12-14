using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class ServingSlot : ContainerObject {

    public int index;
    public Transform customerStartPoint;
    public Transform customerEndPoint;

    //public Transform[] movePointArray;
    // [0], [1]: left or right StartPoint
    // [2]: TurnPoint
    // [3]: EndPoint

    private bool isEngage;
    private CustomerSO currentCustomerSO;
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

                CurrentCustomer = InstantiateCustomer(); // Instantiate customer
                CurrentCustomer.gameObject.SetActive(false); // Show the customer
                StartCoroutine(CustomerArriving()); // Move customer from start point to end point

                CurrentCustomer.CurrentSlot = this; // Set customer CurrentSlot to this slot

                // Subscribe to Customer event
                CurrentCustomer.OnCustomerLeaving += CurrentCustomer_OnCustomerLeaving; 
                CurrentCustomer.OnOrderSpawned += CurrentCustomer_OnOrderSpawned;
                CurrentCustomer.OnOrderCompleted += CurrentCustomer_OnOrderCompleted;
                CurrentCustomer.OnStateChanged += CurrentCustomer_OnStateChanged;

                isEngage = true;
            }
        } else {
            // There is one customer at the moment
        }
    }

    private void CurrentCustomer_OnOrderCompleted(object sender, Customer.OnOrderCompletedEventArgs e) {
        ServingManager.Instance.UpdateOrderSummary(index, true);
        GameHandler.Instance.UpdateIncome(e.completedState, e.totalOrderPrice, e.tipsAmount);
    }

    private void CurrentCustomer_OnStateChanged(object sender, EventArgs e) {
        ServingManager.Instance.UpdateStarVisual(index, CurrentCustomer.GetStarNumberByState());
    }

    private void CurrentCustomer_OnOrderSpawned(object sender, EventArgs e) {
        ServingManager.Instance.UpdateOrderSummary(index, false);
    }

    private IEnumerator CustomerLeaving() {
        /*Debug.Log($"{this.name}[{string.Join(",", this.GetChildrenObject())}]");*/
        if (HasChildrenObject()) {
            StartCoroutine(DestroyChildrenObject(0.01f));
        }
        yield return CurrentCustomer.MoveCustomer(customerEndPoint.position, customerStartPoint.position, Quaternion.identity);

        Destroy(CurrentCustomer.gameObject); // After the customer had moved to end point -> destroy that customer
        ClearCustomer(); // Set the CurrentCustomer of this slot to null

        isEngage = false; // Ready to spawn new customer
    }

    private IEnumerator CustomerArriving() {
        yield return new WaitForSeconds((float)UnityEngine.Random.Range(1, 30));
        CurrentCustomer.gameObject.SetActive(true);
        yield return CurrentCustomer.MoveCustomer(customerStartPoint.position, customerEndPoint.position, Quaternion.Euler(0, 180, 0));
        CurrentCustomer.isArrived = true;
    }

    private void CurrentCustomer_OnCustomerLeaving(object sender, EventArgs e) {
        StartCoroutine(CustomerLeaving());
    }

    private void ClearCustomer() {
        CurrentCustomer = null;
    }

    private Customer InstantiateCustomer() {
        Transform customerTransform = Instantiate(currentCustomerSO.prefab, customerStartPoint.position, Quaternion.identity);
        return customerTransform.GetComponent<Customer>();
    }

}
