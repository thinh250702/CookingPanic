using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;
    // private float volume;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("There is more than one Instance");
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeVolume(float value) {
        audioSource.volume = value;
    }
}
