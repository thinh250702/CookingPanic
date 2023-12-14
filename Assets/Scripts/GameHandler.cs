using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour, IDataPersistence {

    public static GameHandler Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public class OnLevelDataChangedEventArgs : EventArgs {
        public float currentMoney;
        public int guestsNumber;
    }

    public event EventHandler<OnLevelDataChangedEventArgs> OnLevelDataChanged;

    public enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private string[] months = { "January", "February", "March", 
                                "April", "May", "June", 
                                "July", "August", "September", 
                                "October", "November", "December" };

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 60f;
    private bool isGamePaused = false;

    private int levelDay;
    private int levelMonth;

    private float currentMoney;
    private float expenses;
    private float foodSales;
    private float tips;

    private int totalGuests;
    private int perfect;
    private int good;
    private int bad;
    private int lost;

    public LevelData SingleDayData { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Debug.Log("There is more than one instance!");
        }
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start() {
        expenses = 0;
        foodSales = 0;
        tips = 0;
        perfect = 0;
        good = 0;
        bad = 0;
        lost = 0;
        currentMoney = 0;
        totalGuests = 0;

        SingleDayData = new LevelData(); // init the empty level data
        Player.Instance.OnPauseAction += Player_OnPauseAction;
    }

    private void Player_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    public bool UpdateExpenses(float spendAmount) {
        if (currentMoney >= spendAmount) {
            expenses += spendAmount;
            currentMoney -= spendAmount;

            OnLevelDataChanged?.Invoke(this, new OnLevelDataChangedEventArgs {
                currentMoney = this.currentMoney,
                guestsNumber = this.totalGuests,
            });
            return true;
        } else {
            PopupMessageUI.Instance.SetMessage("Not have enough money to restock!");
            return false;
        }
    }

    public void UpdateIncome(int state, float foodSalesAmount, float tipsAmount) {
        switch (state) {
            case 3:
                perfect++;
                break;
            case 2:
                good++;
                break;
            case 1:
                bad++;
                break;
            case 0:
                lost++;
                break;
        }
        totalGuests++;

        foodSales += foodSalesAmount;
        tips += tipsAmount;
        currentMoney += (foodSalesAmount + tipsAmount);

        OnLevelDataChanged?.Invoke(this, new OnLevelDataChangedEventArgs {
            currentMoney = this.currentMoney,
            guestsNumber = this.totalGuests,
        });
        Debug.Log($"LevelData Updating: perfect = {this.perfect}, good = {this.good}, bad = {this.bad}, lost = {this.lost}");
    }

    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f) {
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f) {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f) {

                    // init the level with data after day end
                    SingleDayData = new LevelData(currentMoney, expenses, foodSales, tips, totalGuests, perfect, good, bad, lost);

                    state = State.GameOver;
                    gamePlayingTimer = 0f;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    // save the game
                    DataPersistenceManager.Instance.SaveGame();
                    // transition back to the level selection scene
                    SceneLoader.Load(SceneLoader.Scene.SelectionScene);
                }
                break;
        }
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsNewDayActive() {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized() {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public float GetGamePlayingTimer() {
        return gamePlayingTimer;
    }

    public void LoadData(PlayerData playerData) {
        Debug.Log("Load Level Data!");
        levelDay = playerData.currentDay;
        levelMonth = playerData.currentMonth;
        Debug.Log($"Now playing level: {levelDay}/{levelMonth}");
    }

    public void SaveData(PlayerData playerData) {
        Debug.Log("Save Level Data!");
        playerData.calendarData[months[levelMonth - 1]][levelDay - 1] = SingleDayData;
    }

    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;
        if (isGamePaused) {
            Debug.Log("Pause Game!");
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Debug.Log("Unpause Game!");
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
