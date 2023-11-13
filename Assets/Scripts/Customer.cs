using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : InteractableObject {
    public CustomerSO customerSO;
    public GameObject customerVisual;

    public event EventHandler<EventArgs> OnCustomerLeaving;

    public enum State {
        Walking,
        Idle,
        Perfect,
        Good,
        Bad,
    }

    private Animator animator;
    private bool hasOrdered;
    private int orderItemNumber;
    public List<RecipeSO> orderRecipeSOList = new List<RecipeSO>();
    private RecipeListSO recipeListSO;

    public bool isArrived;
    private bool isFinished;

    private State state;
    private float perfectWaitingTimer;
    private float goodWaitingTimer;
    private float badWaitingTimer;

    private int isArrivedHashed = Animator.StringToHash("isArrived");
    private int perfectHashed = Animator.StringToHash("perfect");
    private int goodHashed = Animator.StringToHash("good");
    private int badHashed = Animator.StringToHash("bad");

    private float speed = 1f;

    public ServingSlot CurrentSlot { get; set; }

    private void Start() {
        animator = customerVisual.GetComponent<Animator>();
        recipeListSO = ServingManager.Instance.GetRecipeListSO();
        perfectWaitingTimer = goodWaitingTimer = badWaitingTimer = customerSO.waitingTime;
        state = State.Walking;
    }

    private void Update() {
        switch (state) {
            case State.Walking:
                if (isArrived) {
                    // The customer has arrived -> Set state to Idle
                    state = State.Idle;
                    animator.SetBool(isArrivedHashed, true);
                } else {
                    animator.SetBool(isArrivedHashed, false);
                    if (animator.GetAnimatorTransitionInfo(0).IsName("Customer_Idle -> Customer_Walking")) {
                        if (isFinished) {
                            Debug.Log("Call event");
                            OnCustomerLeaving?.Invoke(this, EventArgs.Empty);
                            isFinished = false;
                        }
                    }             
                }
                break;
            case State.Idle:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Customer_Idle")) {
                    if (!hasOrdered) {
                        // The customer have not had an order yet
                        //orderItemNumber = UnityEngine.Random.Range(1, 4); // Randomly choose an item number of order
                        orderItemNumber = 1;
                        for (int i = 0; i < orderItemNumber; i++) {
                            // Randomly choose a recipe then add to list
                            orderRecipeSOList.Add(recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)]);
                        }
                        Debug.Log("Number of item = " + orderItemNumber);
                        orderRecipeSOList.ForEach(Debug.Log);
                        hasOrdered = true;
                        state = State.Perfect;
                        Debug.Log("Timer started");
                    } else {
                        if (isFinished) {
                            isArrived = false;
                            state = State.Walking;
                        }
                    }
                }
                break;
            case State.Perfect:
                perfectWaitingTimer -= Time.deltaTime;
                if (perfectWaitingTimer < 0f) {
                    // Out of perfect time
                    perfectWaitingTimer = customerSO.waitingTime; // Reset to default value
                    state = State.Good;
                }
                break;
            case State.Good:
                goodWaitingTimer -= Time.deltaTime;
                if (goodWaitingTimer < 0f) {
                    // Out of good time
                    goodWaitingTimer = customerSO.waitingTime; // Reset to default value
                    state = State.Bad;
                }
                break;
            case State.Bad:
                badWaitingTimer -= Time.deltaTime;
                if (badWaitingTimer < 0f) {
                    // Out of bad time
                    badWaitingTimer = customerSO.waitingTime; // Reset to default value
                    animator.SetTrigger(badHashed);
                    isFinished = true;
                    state = State.Idle;
                }
                break;
        }
        
    }

    public IEnumerator MoveCustomer(Vector3 start, Vector3 end, Quaternion rotation) {
        float elapsedTime = 0;
        float waitTime = (end - start).magnitude / speed;

        while (elapsedTime < waitTime) {
            float percentageComplete = elapsedTime / waitTime;

            this.transform.position = Vector3.Lerp(start, end, percentageComplete);
            this.transform.rotation = rotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = end;
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject() && player.GetChildrenObject()[0] is ContainerObject) {
            ContainerObject container = player.GetChildrenObject()[0] as ContainerObject;
            if (container.GetContainerObjectSO() == ServingManager.Instance.validObjectSO) {
                if (CurrentSlot.ServeOrder(container, orderRecipeSOList)) {
                    // Served correct order
                    Debug.Log("Player delivered the correct order!");
                    switch (state) {
                        case State.Perfect:
                            animator.SetTrigger(perfectHashed);
                            break;
                        case State.Good:
                            animator.SetTrigger(goodHashed);
                            break;
                        case State.Bad:
                            animator.SetTrigger(badHashed);
                            break;
                    }
                } else {
                    // Served incorrect order
                    Debug.Log("Player delivered the incorrect order!");
                    animator.SetTrigger(badHashed);
                }
                isFinished = true;
                state = State.Idle;
            } else {
                Debug.Log("Pick a serving tray to serve meal!");
            }
        }
    }

    public void ClearSlot() {
        CurrentSlot = null;
    }

    public CustomerSO GetCustomerSO() {
        return customerSO;
    }


}
