using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start() {
        timerText.text = "00:00";
        timerImage.fillAmount = 0;
    }

    private void Update() {
        timerImage.fillAmount = GameHandler.Instance.GetGamePlayingTimerNormalized();
        SetTimerText(GameHandler.Instance.GetGamePlayingTimer());
    }

    private void SetTimerText(float remainingTime) {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
