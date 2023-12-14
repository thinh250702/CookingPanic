using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [SerializeField] private SaveSlot[] saveSlots;

    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button loadButton;
    [SerializeField] private Button overrideButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button backButton;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopupUI confirmPopupUI;

    [Header("Slot Colors")]
    [SerializeField] private Color emptyNormal;
    [SerializeField] private Color emptyHighlight;
    [SerializeField] private Color hasDataNormal;
    [SerializeField] private Color hasDataHighlight;
    [SerializeField] private Color hasDataTextNormal;
    [SerializeField] private Color hasDataTextHighlight;

    private SaveSlot selectedSaveSlot;
    private bool isLoadGame = false;

    private void Start() {
        gameObject.SetActive(false);
        ResetSlotsVisual();
    }

    public void OnSaveSlotEnter(SaveSlot saveSlot) {
        ResetSlotsVisual();
        if (saveSlot.hasData) {
            saveSlot.background.color = hasDataHighlight;
            saveSlot.playerNameText.color = hasDataTextHighlight;
        } else {
            saveSlot.background.color = emptyHighlight;
        }
    }

    public void OnSaveSlotExit(SaveSlot saveSlot) {
        ResetSlotsVisual();
    }

    public void OnSaveSlotSelected(SaveSlot saveSlot) {
        selectedSaveSlot = saveSlot;

        ResetSlotsVisual();
        if (saveSlot.hasData) {
            saveSlot.background.color = hasDataHighlight;
            saveSlot.playerNameText.color = hasDataTextHighlight;
        } else {
            saveSlot.background.color = emptyHighlight;
        }

        Debug.Log(this.selectedSaveSlot.GetProfileId());

        DisableButtonsDependingOnData(saveSlot);

        // case - load game mode
        if (isLoadGame) {
            // slot has data
            if (saveSlot.hasData) {
                Debug.Log("Click Load button to load the game data!");
            } 
            // slot has no data - do nothing
            else {
                Debug.Log("There's no data to load!");
            }
        }
        // case - new game mode
        else {
            // slot has data - override
            if (saveSlot.hasData) {
                Debug.Log("There's already had data in this slot. You need to override it!");
            }
            // slot has no data
            else {
                Debug.Log("Ready to Start New Game!");
                confirmPopupUI.Show(
                // function to execute if we select 'yes'
                () => {
                    DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                    DataPersistenceManager.Instance.NewGame();
                    SaveGameAndLoadScene();
                },
                // function to execute if we select 'cancel'
                () => {
                    ShowMenu(isLoadGame);
                });
            }
        }
    }

    private void ResetSlotsVisual() {
        foreach (SaveSlot saveSlot in saveSlots) {
            if (selectedSaveSlot != null && saveSlot == selectedSaveSlot) { continue; }
            if (saveSlot.hasData) {
                saveSlot.background.color = hasDataNormal;
                saveSlot.playerNameText.color = hasDataTextNormal;
            } else {
                saveSlot.background.color = emptyNormal;
            }
        }
    }

    private void ResetAllSlotsVisual() {
        foreach (SaveSlot saveSlot in saveSlots) {
            if (saveSlot.hasData) {
                saveSlot.background.color = hasDataNormal;
                saveSlot.playerNameText.color = hasDataTextNormal;
            } else {
                saveSlot.background.color = emptyNormal;
            }
        }
    }

    private void SaveGameAndLoadScene() {
        // save the game anytime before loading a new scene
        DataPersistenceManager.Instance.SaveGame();
        // load the scene
        SceneLoader.Load(SceneLoader.Scene.SelectionScene);
    }

    public void OnOverrideClicked() {
        Debug.Log("Override Slot");
        confirmPopupUI.Show(
        // function to execute if we select 'yes'
        () => {
            DataPersistenceManager.Instance.ChangeSelectedProfileId(this.selectedSaveSlot.GetProfileId());
            DataPersistenceManager.Instance.NewGame();
            SaveGameAndLoadScene();
        },
        // function to execute if we select 'cancel'
        () => {
            ShowMenu(isLoadGame);
        });
    }

    public void OnDeleteClicked() {
        Debug.Log("Delete Slot");
        confirmPopupUI.Show(
        // function to execute if we select 'yes'
        () => {
            DataPersistenceManager.Instance.DeleteProfileData(this.selectedSaveSlot.GetProfileId());
            ShowMenu(isLoadGame);
        },
        // function to execute if we select 'cancel'
        () => {
            ShowMenu(isLoadGame);
        });
    }

    public void OnBackClicked() {
        mainMenu.ShowMenu();
        HideMenu();
    }

    public void OnLoadClicked() {
        Debug.Log("Ready to Load Game!");
        confirmPopupUI.Show(
        // function to execute if we select 'yes'
        () => {
            DataPersistenceManager.Instance.ChangeSelectedProfileId(this.selectedSaveSlot.GetProfileId());
            SaveGameAndLoadScene();
        },
        // function to execute if we select 'cancel'
        () => {
            ShowMenu(isLoadGame);
        });
    }

    public void ShowMenu(bool isLoadGame) {
        // set mode
        this.isLoadGame = isLoadGame;

        // load all of the profiles that exist
        Dictionary<string, PlayerData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        // loop through each save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots) {
            PlayerData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }

        // case: load game mode
        if (isLoadGame) {
            loadButton.gameObject.SetActive(true);
            overrideButton.gameObject.SetActive(false);
        } 
        // case: new game mode
        else {
            loadButton.gameObject.SetActive(false);
            overrideButton.gameObject.SetActive(true);
        }

        // reset all the slot visual before show menu
        ResetAllSlotsVisual();
        overrideButton.interactable = false;
        deleteButton.interactable = false;
        loadButton.interactable = false;

        // set this menu to be active
        this.gameObject.SetActive(true);
    }

    public void HideMenu() {
        this.gameObject.SetActive(false);
    }

    private void DisableButtonsDependingOnData(SaveSlot saveSlot) {
        if (saveSlot.hasData) {
            overrideButton.interactable = true;
            deleteButton.interactable = true;
            loadButton.interactable = true;
        } else {
            overrideButton.interactable = false;
            deleteButton.interactable = false;
            loadButton.interactable = false;
        }
    }
}
