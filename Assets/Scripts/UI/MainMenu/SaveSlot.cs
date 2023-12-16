using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;

    public Image background;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI lastPlayedText;
    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI currentDateText;

    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    public bool hasData { get; private set; } = false;

    public void SetData(PlayerData data) {
        // there's no data for this profileId
        if (data == null) {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for this profileId
        else {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            playerNameText.text = data.playerName;
            lastPlayedText.text = DateTime.FromBinary(data.lastPlayed).ToString("MM/dd/yyyy");
            totalMoneyText.text = data.currentMoney.ToString();
            currentDateText.text = $"{data.currentDate.Day}/{data.currentDate.Month}";
        }
    }

    public string GetProfileId() {
        return this.profileId;
    }

    public void OnPointerClick(PointerEventData eventData) {
        saveSlotsMenu.OnSaveSlotSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        saveSlotsMenu.OnSaveSlotEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        saveSlotsMenu?.OnSaveSlotExit(this);
    }
}
