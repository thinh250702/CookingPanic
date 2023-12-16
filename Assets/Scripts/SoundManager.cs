using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1f;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("There is more than one Instance");
        }
        Instance = this;
    }

    private void Start() {
        ServingManager.Instance.OnServingSuccess += ServingManager_OnServingSuccess;
        ServingManager.Instance.OnServingFailed += ServingManager_OnServingFailed;
        CuttingBoard.OnAnyCut += CuttingBoard_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        ContainerObject.OnAnyObjectPlacedHere += ContainerObject_OnAnyObjectPlacedHere;
        Trashbin.OnAnyObjectTrashed += Trashbin_OnAnyObjectTrashed;
    }

    private void ContainerObject_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        ContainerObject container = sender as ContainerObject;
        PlaySound(audioClipRefsSO.objectDrop, container.transform.position);
    }

    private void Trashbin_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        Trashbin trashbin = sender as Trashbin;
        PlaySound(audioClipRefsSO.trash, trashbin.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingBoard_OnAnyCut(object sender, System.EventArgs e) {
        CuttingBoard cuttingBoard = sender as CuttingBoard;
        PlaySound(audioClipRefsSO.chop, cuttingBoard.transform.position);
    }

    private void ServingManager_OnServingFailed(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.deliveryFailed, Camera.main.transform.position, .5f);
    }

    private void ServingManager_OnServingSuccess(object sender, System.EventArgs e) {
        PlaySound(audioClipRefsSO.deliverySuccess, Camera.main.transform.position, .5f);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultipler = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultipler * volume);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume = 1f) {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void PlayCustomerSound(CustomerSO.Gender gender, string soundName, Vector3 position, float volume = 1f) {
        var action = new UnityAction(() => { });
        switch (soundName) {
            case "hello":
                action = (gender == CustomerSO.Gender.Male) ? 
                    () => { PlaySound(audioClipRefsSO.maleHello, position, volume); } : 
                    () => { PlaySound(audioClipRefsSO.femaleHello, position, volume); };
                break;
            case "happy":
                action = (gender == CustomerSO.Gender.Male) ?
                    () => { PlaySound(audioClipRefsSO.maleHappy, position, volume); } :
                    () => { PlaySound(audioClipRefsSO.femaleHappy, position, volume); };
                break;
            case "disappoint":
                action = (gender == CustomerSO.Gender.Male) ?
                    () => { PlaySound(audioClipRefsSO.maleDisappoint, position, volume); } :
                    () => { PlaySound(audioClipRefsSO.femaleDisappoint, position, volume); };
                break;
            case "disgust":
                action = (gender == CustomerSO.Gender.Male) ?
                    () => { PlaySound(audioClipRefsSO.maleDisgust, position, volume); } :
                    () => { PlaySound(audioClipRefsSO.femaleDisgust, position, volume); };
                break;
        }
        action();
    }

    public void ChangeVolume(float value) {
        volume = value;
    }
}
