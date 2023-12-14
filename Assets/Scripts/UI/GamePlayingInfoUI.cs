using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayingInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI guestNumberText;

    private void Start() {
        moneyText.text = "0,00 $";
        guestNumberText.text = "0";
        GameHandler.Instance.OnLevelDataChanged += GameHandler_OnLevelDataChanged;
    }

    private void GameHandler_OnLevelDataChanged(object sender, GameHandler.OnLevelDataChangedEventArgs e) {
        moneyText.text = String.Format("{0:0.00} $", e.currentMoney);
        guestNumberText.text = e.guestsNumber.ToString();
    }
}
