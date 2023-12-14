using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DynamicCalendarSingleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public enum CellType {
        Inactive,
        Active
    }

    [Header("Day Cell UI Components")]
    public TextMeshProUGUI dateText;
    public Image background;
    public Image lockOverlay;
    public Transform subInfo;
    public TextMeshProUGUI totalEarnedText;
    public TextMeshProUGUI guestsText;

    public CellType Type { get; set; }
    public int Month { get; private set; }
    public int Day { get; private set; }

    public LevelData DayRecapData { get; set; }

    // Constructor
    public void SetCellInfo(int day, int month, CellType type) {
        this.Day = day;
        this.Month = month;
        this.Type = type;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (Type == CellType.Active) {
            DynamicCalendarUI.Instance.OnCellEnter(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (Type == CellType.Active) {
            DynamicCalendarUI.Instance.OnCellExit(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (Type == CellType.Active) {
            DynamicCalendarUI.Instance.OnCellSelected(this);
        }
    }
}
