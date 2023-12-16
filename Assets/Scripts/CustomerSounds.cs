using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSounds : MonoBehaviour
{
    private Customer customer;

    private void Awake() {
        customer = GetComponent<Customer>();
    }

    private void Start() {
        customer.OnPlayingSound += Customer_OnPlayingSound;
    }

    private void Customer_OnPlayingSound(object sender, Customer.OnPlayingSoundEventArgs e) {
        SoundManager.Instance.PlayCustomerSound(customer.GetCustomerSO().gender, e.soundName, Camera.main.transform.position);
    }
}
