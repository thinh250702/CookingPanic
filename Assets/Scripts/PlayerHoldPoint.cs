using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHoldPoint : MonoBehaviour
{
    [SerializeField] private Transform playerCameraRoot;

    private Vector3 defaultPosition;

    private void Start() {
        defaultPosition = transform.localPosition;
    }

    private void Update() {
        transform.localRotation = Quaternion.Euler(-playerCameraRoot.transform.localEulerAngles.x, 0, 0);
    }
}
