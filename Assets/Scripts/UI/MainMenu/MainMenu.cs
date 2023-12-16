using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private OptionsMenu optionsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private TextMeshProUGUI welcomeText;
    private void Awake() {
        continueButton.onClick.AddListener(() => {
            Debug.Log("ContinueGame");
            // save the game anytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // load the next scene - which will in turn load the game because of OnSceneLoaded() in the DataPersistenceManager
            SceneLoader.Load(SceneLoader.Scene.SelectionScene);
        });
        newGameButton.onClick.AddListener(() => {
            Debug.Log("NewGame");
            saveSlotsMenu.ShowMenu(false);
            this.HideMenu();
        });
        loadGameButton.onClick.AddListener(() => {
            Debug.Log("LoadGame");
            saveSlotsMenu.ShowMenu(true);
            this.HideMenu();
        });
        optionsButton.onClick.AddListener(() => {
            optionsMenu.ShowMenu();
            HideMenu();
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start() {
        welcomeText.text = "";
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData() {
        if (!DataPersistenceManager.Instance.HasGameData()) {
            Debug.Log("No game data");
            continueButton.interactable = false;
            loadGameButton.interactable = false;
            welcomeText.gameObject.SetActive(false);
        } else {
            Debug.Log("Has game data");
            welcomeText.text = $"Welcome, {DataPersistenceManager.Instance.GetRecentlyPlayerName()}";
        }
    }

    public void ShowMenu() {
        DisableButtonsDependingOnData();
        this.gameObject.SetActive(true);
    }

    public void HideMenu() {
        this.gameObject.SetActive(false);
    }
}
