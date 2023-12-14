using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServingManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderNumberText;

    [SerializeField] private Transform orderContainer;
    [SerializeField] private Transform orderRowTemplate;

    private void Awake() {
        /*orderRowTemplate.gameObject.SetActive(false);*/
        foreach (Transform child in orderContainer) {
            child.gameObject.SetActive(false);
            /*Destroy(child.gameObject);*/
        }
    }

    private void Start() {
        
        ServingManager.Instance.OnOrderChanged += Instance_OnOrderChanged;
        ServingManager.Instance.OnStarChanged += Instance_OnStarChanged;
    }

    private void Instance_OnOrderChanged(object sender, ServingManager.OnOrderChangedEventArgs e) {
        GameObject orderRowChildren = orderContainer.transform.GetChild(e.slotIndex).gameObject;
        var slot = ServingManager.Instance.GetServingSlotList()[e.slotIndex];
        if (!e.isCompleted) {
            if (slot.CurrentCustomer.GetCustomerOrderList().Count > 0) {
                /*orderNumberText.text = $"Slot {e.slotIndex + 1}";*/
                orderRowChildren.gameObject.SetActive(true);
                orderRowChildren.GetComponent<ServingManagerSingleUI>().SetRecipeSO(slot.CurrentCustomer.GetCustomerOrderList());
            }
        } else {
            orderRowChildren.gameObject.SetActive(false);
        }
    }

    private void Instance_OnStarChanged(object sender, ServingManager.OnStarChangedEventArgs e) {
        GameObject orderRowChildren = orderContainer.transform.GetChild(e.slotIndex).gameObject;
        orderRowChildren.GetComponent<ServingManagerSingleUI>().SetStarVisual(e.starNumber);
    }
}
