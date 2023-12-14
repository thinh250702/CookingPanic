using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Customer : InteractableObject {
    public CustomerSO customerSO;
    public GameObject customerVisual;

    public class OnOrderCompletedEventArgs : EventArgs {
        public int completedState;
        public float totalOrderPrice;
        public float tipsAmount;
    }

    public event EventHandler<EventArgs> OnCustomerLeaving;

    public event EventHandler<EventArgs> OnOrderSpawned;
    public event EventHandler<OnOrderCompletedEventArgs> OnOrderCompleted;

    public event EventHandler<EventArgs> OnStateChanged;

    public enum State {
        Walking,
        Idle,
        Perfect,
        Good,
        Bad,
    }

    public ServingSlot CurrentSlot { get; set; }

    private Animator animator;
    private bool hasOrdered;
    private int orderItemNumber;
    private List<RecipeSO> orderRecipeSOList = new List<RecipeSO>();
    private RecipeListSO recipeListSO;

    [HideInInspector]
    public bool isArrived;
    private bool isFinished;

    private State state;
    private float perfectWaitingTimer;
    private float goodWaitingTimer;
    private float badWaitingTimer;

    private float totalOrderPrice;

    private int isArrivedHashed = Animator.StringToHash("isArrived");
    private int perfectHashed = Animator.StringToHash("perfect");
    private int goodHashed = Animator.StringToHash("good");
    private int badHashed = Animator.StringToHash("bad");

    private float speed = 1f;

    private void Start() {
        animator = customerVisual.GetComponent<Animator>();
        recipeListSO = ServingManager.Instance.GetRecipeListSO();
        perfectWaitingTimer = goodWaitingTimer = badWaitingTimer = customerSO.waitingTime;
        state = State.Walking;
    }

    public int GetStarNumberByState() {
        switch (state) {
            case State.Perfect:
                return 3;
            case State.Good:
                return 2;
            case State.Bad:
                return 1;
        }
        return -1;
    }

    public List<RecipeSO> GetCustomerOrderList() {
        return orderRecipeSOList;
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
                        // orderItemNumber = UnityEngine.Random.Range(1, 4); // Randomly choose an item number of order
                        orderItemNumber = 1;
                        for (int i = 0; i < orderItemNumber; i++) {
                            // Randomly choose a recipe then add to list
                            RecipeSO recipe = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                            orderRecipeSOList.Add(recipe);
                            totalOrderPrice += recipe.price;
                        }
                        OnOrderSpawned?.Invoke(this, EventArgs.Empty);
                        hasOrdered = true;
                        state = State.Perfect;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    } else {
                        if (isFinished) {
                            isArrived = false;
                            state = State.Walking;
                            orderRecipeSOList.Clear();
                            totalOrderPrice = 0;
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
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Good:
                goodWaitingTimer -= Time.deltaTime;
                if (goodWaitingTimer < 0f) {
                    // Out of good time
                    goodWaitingTimer = customerSO.waitingTime; // Reset to default value
                    state = State.Bad;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
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
                    OnOrderCompleted?.Invoke(this, new OnOrderCompletedEventArgs {
                        completedState = 0,
                        totalOrderPrice = 0,
                        tipsAmount = 0
                    });
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
        if (state == State.Perfect || state == State.Good || state == State.Bad) {
            if (player.HasChildrenObject() && player.GetChildrenObject()[0] is ContainerObject) {
                ContainerObject container = player.GetChildrenObject()[0] as ContainerObject;
                if (container.GetContainerObjectSO() == ServingManager.Instance.validObjectSO) {
                    container.DropConcaveContainer(CurrentSlot, CurrentSlot.transform.position, Quaternion.identity);
                    if (ServingManager.Instance.ServeOrder(container, orderRecipeSOList)) {
                        // Served correct order
                        Debug.Log("Player delivered the correct order!");
                        switch (state) {
                            case State.Perfect:
                                animator.SetTrigger(perfectHashed);
                                OnOrderCompleted?.Invoke(this, new OnOrderCompletedEventArgs {
                                    completedState = 3,
                                    totalOrderPrice = this.totalOrderPrice,
                                    tipsAmount = customerSO.maxTip
                                });
                                break;
                            case State.Good:
                                animator.SetTrigger(goodHashed);
                                OnOrderCompleted?.Invoke(this, new OnOrderCompletedEventArgs {
                                    completedState = 2,
                                    totalOrderPrice = this.totalOrderPrice,
                                    tipsAmount = (float)customerSO.maxTip / 2
                                });
                                break;
                            case State.Bad:
                                animator.SetTrigger(badHashed);
                                OnOrderCompleted?.Invoke(this, new OnOrderCompletedEventArgs {
                                    completedState = 1,
                                    totalOrderPrice = this.totalOrderPrice,
                                    tipsAmount = 0
                                });
                                break;
                        }
                    } else {
                        // Served incorrect order
                        Debug.Log("Player delivered the incorrect order!");
                        OnOrderCompleted?.Invoke(this, new OnOrderCompletedEventArgs {
                            completedState = 0,
                            totalOrderPrice = 0,
                            tipsAmount = 0
                        });
                        animator.SetTrigger(badHashed);
                    }
                    isFinished = true;
                    state = State.Idle;

                } else {
                    Debug.Log("Pick a serving tray to serve meal!");
                }
            }
        } else {
            Debug.Log("Can not Interact!");
        }
    }

    public CustomerSO GetCustomerSO() {
        return customerSO;
    }


}
