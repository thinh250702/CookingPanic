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

    public Customer currentCustomer { get; set; }

    private void Start() {
        customerListSO = ServingManager.Instance.GetCustomerListSO();   
    }

    private void Update() {
        if (!isEngage) {
            // There is no customer at the moment -> Spawn customer
            if (currentCustomer == null && !GameHandler.Instance.IsGameOver()) {
                // Create customer randomly
                this.currentCustomerSO = customerListSO.customerSOList[UnityEngine.Random.Range(0, customerListSO.customerSOList.Count)];

                currentCustomer = InstantiateCustomer(); // Instantiate customer
                currentCustomer.gameObject.SetActive(false); // Show the customer/
                StartCoroutine(CustomerArriving()); // Move customer from start point to end point

                currentCustomer.currentSlot = this; // Set customer CurrentSlot to this slot

                // Subscribe to Customer event
                currentCustomer.OnCustomerLeaving += CurrentCustomer_OnCustomerLeaving; 
                currentCustomer.OnOrderSpawned += CurrentCustomer_OnOrderSpawned;
                currentCustomer.OnOrderCompleted += CurrentCustomer_OnOrderCompleted;
                currentCustomer.OnStateChanged += CurrentCustomer_OnStateChanged;

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
        ServingManager.Instance.UpdateStarVisual(index, currentCustomer.GetStarNumberByState());
    }

    private void CurrentCustomer_OnOrderSpawned(object sender, EventArgs e) {
        ServingManager.Instance.UpdateOrderSummary(index, false);
    }

    private IEnumerator CustomerLeaving() {
        if (HasChildrenObject()) {
            StartCoroutine(DestroyChildrenObject(0.01f));
        }
        yield return currentCustomer.MoveCustomer(customerEndPoint.position, customerStartPoint.position, Quaternion.identity);

        Destroy(currentCustomer.gameObject); // After the customer had moved to end point -> destroy that customer
        ClearCustomer(); // Set the CurrentCustomer of this slot to null

        isEngage = false; // Ready to spawn new customer
    }

    private IEnumerator CustomerArriving() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        //yield return new WaitForSeconds(1f);
        currentCustomer.gameObject.SetActive(true);
        yield return currentCustomer.MoveCustomer(customerStartPoint.position, customerEndPoint.position, Quaternion.Euler(0, 180, 0));
        currentCustomer.isArrived = true;
    }

    private void CurrentCustomer_OnCustomerLeaving(object sender, EventArgs e) {
        StartCoroutine(CustomerLeaving());
    }

    private void ClearCustomer() {
        currentCustomer = null;
    }

    private Customer InstantiateCustomer() {
        Transform customerTransform = Instantiate(currentCustomerSO.prefab, customerStartPoint.position, Quaternion.identity);
        return customerTransform.GetComponent<Customer>();
    }

}
