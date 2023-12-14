using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    public string playerName;
    public long lastPlayed;
    public int currentDay;
    public int currentMonth;
    public int currentMoney;
    public Dictionary<string, List<LevelData>> calendarData;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to Load
    public PlayerData() {
        this.playerName = "";
        this.currentDay = 1;
        this.currentMonth = 1;
        this.currentMoney = 0;
        this.calendarData = new Dictionary<string, List<LevelData>>();
    }
}
