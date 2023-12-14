using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public float expenses;
    public float foodSales;
    public float tips;
    public float totalEarned;
    public int totalGuests;
    public int[] detailServed = new int[4];
    public int serviceQuality;

    public LevelData() {
        this.expenses = 0;
        this.foodSales = 0;
        this.tips = 0;
        this.totalEarned = 0;
        this.totalGuests = 0;
        this.detailServed = new int[4];
        this.serviceQuality = -1;
    }

    public LevelData(float totalEarned, float expenses, float foodSales, float tips, int totalGuests, int perfect, int good, int bad, int lost) {
        this.expenses = expenses;
        this.foodSales = foodSales;
        this.tips = tips;
        this.totalEarned = totalEarned;
        this.totalGuests = totalGuests;
        this.detailServed[0] = perfect;
        this.detailServed[1] = good;
        this.detailServed[2] = bad;
        this.detailServed[3] = lost;
        this.serviceQuality = this.GetServiceQuality();
    }

    public int GetServiceQuality() {
        float perfectProportion = (float)this.detailServed[0] / (float)this.totalGuests;
        float goodProportion = (float)this.detailServed[1] / (float)this.totalGuests;
        float badProportion = (float)(this.detailServed[2] + this.detailServed[3]) / (float)this.totalGuests;

        if (perfectProportion >= goodProportion && perfectProportion >= badProportion) {
            return 3;
        } else if (goodProportion >= perfectProportion && goodProportion >= badProportion) {
            return 2;
        } else {
            return 1;
        }
    }
}
