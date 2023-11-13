using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CustomerSO : ScriptableObject
{
    public string customerName;
    public Transform prefab;
    public float waitingTime;
    public int maxTip;
}
