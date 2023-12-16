using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CustomerSO : ScriptableObject
{
    public enum Gender {
        Male, Female
    }

    public string customerName;
    public Gender gender;
    public Transform prefab;
    public float waitingTime;
    public float maxTip;
}
