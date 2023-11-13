using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class StockObjectSO : ScriptableObject {
    public string stockName;
    public Transform prefab;
    public int quantity = 5;
}
