using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour, IParentObject {
    public static Player Instance { get; private set; }

    // event to play sound effect
    public event EventHandler OnPickedSomething;

    public event EventHandler<EventArgs> OnPauseAction;
    public event EventHandler<OnSelectedObjectChangedEventArgs> OnSelectedObjectChanged;
    public class OnSelectedObjectChangedEventArgs : EventArgs {
        public InteractableObject selectedObject;
    }

    [SerializeField] private Transform objectHoldPoint;
    [SerializeField] private Transform cameraRoot;

    private InteractableObject selectedObject;
    private List<PickableObject> holdingObjectList = new List<PickableObject>();
    private RaycastHit raycastHit;
    private StarterAssetsInputs _input;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("There is more than one player");
        }
        Instance = this;
    }

    private void Start() {
        _input = GetComponent<StarterAssetsInputs>();
        _input.cursorInputForLook = false;
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e) {
        if (GameHandler.Instance.IsGamePlaying()) {
            _input.cursorInputForLook = true;
        }
    }

    private void Update() {
        Interact();
        InteractAlternate();
        HandleInteractions();
        HandlePauseGame();
    }

    private void Interact() {
        if (_input.interact) {
            if (selectedObject != null) {
                selectedObject.Interact(this);

            }
            _input.interact = false;
        }
    }

    private void InteractAlternate() {
        if (_input.interactAlternate) {
            if (selectedObject != null) {
                selectedObject.InteractAlternate(this);
            }
            _input.interactAlternate = false;
        }
    }

    public bool IsWalking() {
        return _input.move != Vector2.zero;
    }

    private void HandlePauseGame() {
        if (_input.pause) {
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }
        _input.pause = false;
    }

    private void HandleInteractions() {
        float interactDistance = 5f;
        if (Physics.Raycast(cameraRoot.transform.position, cameraRoot.transform.forward, out raycastHit, interactDistance)) {
            if (raycastHit.transform.TryGetComponent(out InteractableObject interactableObject)) {
                if (interactableObject != selectedObject) {
                    SetSelectedObject(interactableObject);
                }
            } else {
                SetSelectedObject(null);
            }
        } else {
            SetSelectedObject(null);
        }
        //Debug.DrawRay(cameraRoot.transform.position, cameraRoot.transform.forward * interactDistance, Color.red);
    }

    private void SetSelectedObject(InteractableObject selectedObject) {
        this.selectedObject = selectedObject;
        // Phat di su kien
        OnSelectedObjectChanged?.Invoke(this, new OnSelectedObjectChangedEventArgs {
            selectedObject = selectedObject
        });
    }

    public RaycastHit GetPlayerRaycastHit() {
        return this.raycastHit;
    }

    public List<PickableObject> GetChildrenObject() {
        return holdingObjectList;
    }

    public Transform GetObjectFollowTransform() {
        return objectHoldPoint;
    }

    public bool HasChildrenObject() {
        return holdingObjectList.Count != 0;
    }

    public void AddChildrenObject(PickableObject pickableObject) {
        holdingObjectList.Add(pickableObject);

        if (pickableObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveChildrenObject(PickableObject pickableObject) {
        Debug.Log("Clear Object List");
        foreach (var item in holdingObjectList.ToArray()) {
            if (item == pickableObject) {
                holdingObjectList.Remove(item);
            }
        }
    }

    public void ClearAllChildrenObject() {
        holdingObjectList.Clear();
    }
}
