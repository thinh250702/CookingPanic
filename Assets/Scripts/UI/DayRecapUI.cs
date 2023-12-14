using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayRecapUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI expensesText;
    [SerializeField] private TextMeshProUGUI foodSalesText;
    [SerializeField] private TextMeshProUGUI tipsText;
    [SerializeField] private TextMeshProUGUI totalEarnedText;

    [SerializeField] private TextMeshProUGUI totalGuestsText;
    [SerializeField] private List<Transform> containerList;
    [SerializeField] private Transform guestIcon;

    [SerializeField] private Image serviceQualityLabel;
    [SerializeField] private Sprite perfectLabel;
    [SerializeField] private Sprite goodLabel;
    [SerializeField] private Sprite badLabel;

    private void Start() {
        guestIcon.gameObject.SetActive(false);
    }

    public void SetDayRecapData(LevelData levelData) {
        expensesText.text = String.Format("- ${0:0.00}", levelData.expenses);
        foodSalesText.text = String.Format("${0:0.00}", levelData.foodSales);
        tipsText.text = String.Format("${0:0.00}", levelData.tips);
        totalEarnedText.text = String.Format("${0:0.00}", levelData.totalEarned);

        totalGuestsText.text = $"{levelData.totalGuests}";

        for (int i = 0; i < containerList.Count; i++) {
            foreach (Transform child in containerList[i]) {
                if (child == guestIcon) continue;
                Destroy(child.gameObject);
            }
            int count = levelData.detailServed[i];
            for (int j = 0; j < count; j++) {
                Transform guestIconTransform = Instantiate(guestIcon, containerList[i]);
                guestIconTransform.gameObject.SetActive(true);
            }
        }

        switch (levelData.serviceQuality) {
            case 3:
                serviceQualityLabel.sprite = perfectLabel;
                break;
            case 2:
                serviceQualityLabel.sprite = goodLabel;
                break;
            case 1:
                serviceQualityLabel.sprite = badLabel;
                break;
            case -1:
                serviceQualityLabel.sprite = null;
                break;
        }
    }
}
