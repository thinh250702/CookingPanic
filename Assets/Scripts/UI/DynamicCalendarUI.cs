using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCalendarUI : MonoBehaviour, IDataPersistence
{
    public static DynamicCalendarUI Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI monthText;
    [SerializeField] private TextMeshProUGUI dayRecapText;
    [SerializeField] private Transform dateContainer;
    [SerializeField] private Transform dateTemplate;
    [SerializeField] private Transform dayRecapUI;

    [Header("Background Colors")]
    [SerializeField] private Color hoverBackgroundColor;
    [SerializeField] private Color activeBackgroundColor;
    [SerializeField] private Color inactiveBackgroundColor;
    [SerializeField] private Color completedBackgroundColor;

    [Header("Text Colors")]
    [SerializeField] private Color hoverDayTextColor;
    [SerializeField] private Color inactiveDayTextColor;
    [SerializeField] private Color activeDayTextColor;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopupUI confirmPopupUI;

    private int currYear = 2023;
    private int currMonth = 5;

    private int currLevelDay = 1;
    private int currLevelMonth = 1;

    private string[] months = { "January", "February", "March", 
                                "April", "May", "June", 
                                "July", "August", "September", 
                                "October", "November", "December" };

    private List<DynamicCalendarSingleUI> activeCellGroup;
    private DynamicCalendarSingleUI selectedCell;

    private Dictionary<string, List<LevelData>> calendarData;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("There is more than one Instance!");
        }
        Instance = this;
    }

    private void Start() {
        dateTemplate.gameObject.SetActive(false);
        activeCellGroup = new List<DynamicCalendarSingleUI>();

        monthText.text = months[currMonth - 1].ToUpper();
        RenderCalendar();
    }

    public void NextMonth() {
        if (currMonth < 12) {
            currMonth++;
        }
        HandleDateTime();
        RenderCalendar();
    }

    public void PrevMonth() {
        if (currMonth > 1) {
            currMonth--;
        }
        HandleDateTime();
        RenderCalendar();
    }

    private void HandleDateTime() {
        if (currMonth < 1) {
            currYear--;
            currMonth = 12;
        } else if (currMonth > 12) {
            currYear++;
            currMonth = 1;
        } else {
            // do nothing
        }
        monthText.text = months[currMonth - 1].ToUpper();
    }

    private Dictionary<string, List<LevelData>> InitializeCalendarData() {
        Dictionary<string, List<LevelData>> dictionary = new Dictionary<string, List<LevelData>>();
        for (int i = 1; i <= 12; i++) {
            dictionary.Add(months[i - 1], new List<LevelData>());
            for (int j = 0; j < DateTime.DaysInMonth(currYear, i); j++) {
                dictionary[months[i - 1]].Add(new LevelData());
            }
        }
        return dictionary;
    }

    

    private void SetCellVisual(DynamicCalendarSingleUI dayCell) {
        dayCell.dateText.text = dayCell.Day.ToString();

        // active cell visual
        if (dayCell.Type == DynamicCalendarSingleUI.CellType.Active) {
            dayCell.dateText.color = activeDayTextColor;
            // case: active cell is empty (no data = incompleted day)
            if (dayCell.DayRecapData.IsLevelEmpty()) {
                dayCell.background.color = activeBackgroundColor;
                dayCell.subInfo.gameObject.SetActive(false);
                // case: incompleted day is current day - hide the lock
                if (dayCell.Day == currLevelDay && dayCell.Month == currLevelMonth) {
                    dayCell.lockOverlay.gameObject.SetActive(false);
                }
                // case: incompleted day is not current day - show the lock and disable mouse event
                else {
                    dayCell.lockOverlay.gameObject.SetActive(true);
                    dayCell.Type = DynamicCalendarSingleUI.CellType.Inactive;
                }
            }
            // case: active cell is not empty (has data = completed day) - set background to light yellow
            else {
                dayCell.background.color = completedBackgroundColor;
                dayCell.subInfo.gameObject.SetActive(true);
                dayCell.lockOverlay.gameObject.SetActive(false);
                dayCell.totalEarnedText.text = String.Format("${0:0.00}", dayCell.DayRecapData.totalEarned);
                dayCell.guestsText.text = dayCell.DayRecapData.totalGuests.ToString();
            }
        } 
        // inactive cell visual
        else {
            dayCell.dateText.color = inactiveDayTextColor;
            dayCell.background.color = inactiveBackgroundColor;
            dayCell.subInfo.gameObject.SetActive(false);
            dayCell.lockOverlay.gameObject.SetActive(false);
        }
    }

    public void RenderCalendar() {
        foreach (Transform child in dateContainer) {
            if (child == dateTemplate) continue;
            Destroy(child.gameObject);
        }

        int lastMonth = -1;
        if (currMonth == 1) {
            lastMonth = 12;
        } else {
            lastMonth = currMonth - 1;
        }

        var firstDayOfMonth = (int)new DateTime(currYear, currMonth, 1).DayOfWeek;
        var lastDateOfMonth = DateTime.DaysInMonth(currYear, currMonth);
        var lastDayOfMonth = (int)new DateTime(currYear, currMonth, DateTime.DaysInMonth(currYear, currMonth)).DayOfWeek;
        var lastDateOfLastMonth = DateTime.DaysInMonth(currYear, lastMonth);
            
        for (int i = firstDayOfMonth; i > 0; i--) {
            // inactive cell
            Transform dateTemplateTransform = Instantiate(dateTemplate, dateContainer);
            dateTemplateTransform.gameObject.SetActive(true);

            DynamicCalendarSingleUI dayCell = dateTemplateTransform.GetComponent<DynamicCalendarSingleUI>();
            dayCell.SetCellInfo(lastDateOfLastMonth - i + 1, currMonth, DynamicCalendarSingleUI.CellType.Inactive);
            SetCellVisual(dayCell);
        }

        for (int i = 1; i <= lastDateOfMonth; i++) {
            // active cell
            Transform dateTemplateTransform = Instantiate(dateTemplate, dateContainer);
            dateTemplateTransform.gameObject.SetActive(true);

            DynamicCalendarSingleUI dayCell = dateTemplateTransform.GetComponent<DynamicCalendarSingleUI>();
            activeCellGroup.Add(dayCell);

            dayCell.SetCellInfo(i, currMonth, DynamicCalendarSingleUI.CellType.Active);
            dayCell.DayRecapData = calendarData[months[currMonth - 1]][i - 1];

            SetCellVisual(dayCell);
        }

        for (int i = lastDayOfMonth; i < 6; i++) {
            // inactive cell
            Transform dateTemplateTransform = Instantiate(dateTemplate, dateContainer);
            dateTemplateTransform.gameObject.SetActive(true);

            DynamicCalendarSingleUI dayCell = dateTemplateTransform.GetComponent<DynamicCalendarSingleUI>();
            dayCell.SetCellInfo(i - lastDayOfMonth + 1, currMonth, DynamicCalendarSingleUI.CellType.Inactive);
            SetCellVisual(dayCell);
        }
    }

    public void OnCellEnter(DynamicCalendarSingleUI dayCell) {
        ResetCells();
        dayCell.background.color = hoverBackgroundColor;
        dayCell.dateText.color = hoverDayTextColor;
    }

    public void OnCellExit(DynamicCalendarSingleUI dayCell) {
        ResetCells();
    }

    public void OnCellSelected(DynamicCalendarSingleUI dayCell) {
        selectedCell = dayCell;
        ResetCells();

        dayCell.background.color = hoverBackgroundColor;
        dayCell.dateText.color = hoverDayTextColor;

        dayRecapText.text = $"DAY RECAP: {dayCell.Day} {months[dayCell.Month - 1]}";
        dayRecapUI.GetComponent<DayRecapUI>().SetDayRecapData(dayCell.DayRecapData);

        // case: current cell - show the confirm box adn ready to start game
        if (dayCell.Day == currLevelDay && dayCell.Month == currLevelMonth) {
            confirmPopupUI.Show(
            // function to execute if we select 'yes'
            () => {
                Debug.Log("Ready to start the game!");
                // save the game before load
                DataPersistenceManager.Instance.SaveGame();
                // load the game play scene
                SceneLoader.Load(SceneLoader.Scene.MainScene);
            },
            // function to execute if we select 'cancel'
            () => {
                Debug.Log("Cancel button pressed!");
            });
        } else {
            // do nothing
        }
    }

    private void ResetCells() {
        foreach (DynamicCalendarSingleUI cell in activeCellGroup) {
            if (selectedCell != null && cell == selectedCell) { continue; }
            if (cell.background != null) {
                cell.background.color = activeBackgroundColor;
                cell.dateText.color = activeDayTextColor;
            }
        }
    }

    public void OnBackClicked() {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }

    public void LoadData(PlayerData playerData) {
        currLevelMonth = currMonth = playerData.currentMonth;
        currLevelDay = playerData.currentDay;

        Debug.Log("Load Calendar Data!");
        if (playerData.calendarData.Count == 0) {
            Debug.Log("Empty calendar!");
            calendarData = InitializeCalendarData();
        } else {
            Debug.Log("Not empty calendar!");
            calendarData = playerData.calendarData;
        }
        string jsonData = JsonConvert.SerializeObject(calendarData, Formatting.Indented);
        Debug.Log(jsonData);
    }

    public void SaveData(PlayerData playerData) {
        Debug.Log("Save Calendar Data!");
        playerData.calendarData = calendarData;
    }
}
