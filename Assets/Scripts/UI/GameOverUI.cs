using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI expensesText;
    [SerializeField] private TextMeshProUGUI foodSalesText;
    [SerializeField] private TextMeshProUGUI tipsText;
    [SerializeField] private TextMeshProUGUI totalEarnedText;

    [SerializeField] private TextMeshProUGUI totalGuestsText;
    [SerializeField] private List<Transform> containerList;
    [SerializeField] private Transform guestIcon;

    [SerializeField] private TextMeshProUGUI serviceQualityText;
    [SerializeField] private Color perfectColor;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color badColor;

    private void Start() {
        guestIcon.gameObject.SetActive(false);
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        Hide();
    }

    private void GameHandler_OnStateChanged(object sender, System.EventArgs e) {
        if (GameHandler.Instance.IsGameOver()) {
            Show(GameHandler.Instance.SingleDayData);
        } else {
            Hide();
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show(LevelData levelData) {
        expensesText.text = String.Format("- ${0:0.00}", levelData.expenses);
        foodSalesText.text = String.Format("${0:0.00}", levelData.foodSales);
        tipsText.text = String.Format("${0:0.00}", levelData.tips);
        totalEarnedText.text = String.Format("${0:0.00}", levelData.totalEarned);

        totalGuestsText.text = $"{levelData.totalGuests}";

        for (int i = 0; i < containerList.Count; i++)
        {
            int count = levelData.detailServed[i];
            for (int j = 0; j < count; j++)
            {
                Transform guestIconTransform = Instantiate(guestIcon, containerList[i]);
                guestIconTransform.gameObject.SetActive(true);
            }
        }

        switch (levelData.serviceQuality) {
            case 3:
                serviceQualityText.text = "GREAT";
                serviceQualityText.color = perfectColor;
                break;
            case 2:
                serviceQualityText.text = "GOOD";
                serviceQualityText.color = goodColor;
                break;
            case 1:
                serviceQualityText.text = "BAD";
                serviceQualityText.color = badColor;
                break;
        }

        gameObject.SetActive(true);

    }
}
