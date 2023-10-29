using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField] private Transform playerCameraRoot;

    private void Update() {
        transform.localRotation = Quaternion.Euler(-playerCameraRoot.transform.localEulerAngles.x, 0, 0);
    }
}
