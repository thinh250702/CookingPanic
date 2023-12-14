using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button quitButton;

    private void Start() {
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData() {
        if (!DataPersistenceManager.Instance.HasGameData()) {
            Debug.Log("No game data");
            continueButton.interactable = false;
            loadGameButton.interactable = false;
        } else {
            Debug.Log("Has game data");
        }
    }

    public void OnNewGameClicked() {
        Debug.Log("NewGame");
        saveSlotsMenu.ShowMenu(false);
        this.HideMenu();
    }

    public void OnContinueGameClicked() {
        Debug.Log("ContinueGame");
        // save the game anytime before loading a new scene
        DataPersistenceManager.Instance.SaveGame();
        // load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistenceManager
        SceneLoader.Load(SceneLoader.Scene.SelectionScene);
    }

    public void OnLoadGameClicked() {
        Debug.Log("LoadGame");
        saveSlotsMenu.ShowMenu(true);
        this.HideMenu();
    }

    public void OnQuitGameClicked() {
        Application.Quit();
    }

    public void ShowMenu() {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
        /*
        There is no saved data at all -> disable LoadGame and Continue
        There more than one saved data -> enable Load and Continue
         */
    }

    public void HideMenu() {
        this.gameObject.SetActive(false);
    }
}
