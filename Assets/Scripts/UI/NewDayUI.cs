using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class NewDayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI monthText;

    private string[] months = { "January", "February", "March",
                                "April", "May", "June",
                                "July", "August", "September",
                                "October", "November", "December" };

    private void Start() {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        Hide();
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e) {
        if (GameHandler.Instance.IsNewDayActive()) {
            dayText.text = $"DAY {GameHandler.Instance.levelDate.Day.ToString("D2")}";
            monthText.text = months[GameHandler.Instance.levelDate.Month - 1].ToUpper();
            Show();
        } else {
            Hide();
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
