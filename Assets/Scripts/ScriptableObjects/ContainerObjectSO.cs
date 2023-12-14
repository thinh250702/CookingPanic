using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ContainerObjectSO : ScriptableObject {
    public string containerName;
    public Transform prefab;
    public Sprite sprite;
    public string description = "This is for description";
}
