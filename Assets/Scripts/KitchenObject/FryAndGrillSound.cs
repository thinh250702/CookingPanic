using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryAndGrillSound : MonoBehaviour
{
    [SerializeField] private FunctionalSlot functionalSlot;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        functionalSlot.OnStateChanged += GrillSlot_OnStateChanged;
    }

    private void GrillSlot_OnStateChanged(object sender, GrillSlot.OnStateChangedEventArgs e) {
        bool playSound = e.state == GrillSlot.State.Frying || e.state == GrillSlot.State.Fried;
        if (playSound) {
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }
}
