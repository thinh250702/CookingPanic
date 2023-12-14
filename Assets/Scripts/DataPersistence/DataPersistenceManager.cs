using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    public static DataPersistenceManager Instance { get; private set; }

    private PlayerData playerData;

    private FileDataHandler dataHandler;
    private List<IDataPersistence> dataPersistenceObjects;

    private string selectedProfileId = "test";

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Data Persistence Manager!");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence) {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        // C:\Users\1052 DMX\AppData\LocalLow\DefaultCompany\SampleProject
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, "player.json");

        InitializeSelectedProfileId();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 1) {
            Debug.Log("LevelSelection is loaded!");
            Cursor.lockState = CursorLockMode.None;
        }
        if (scene.buildIndex == 2) {
            Debug.Log("GameScene is loaded!");
            Cursor.lockState = CursorLockMode.Locked;
        }
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame() {
        this.playerData = new PlayerData();
    }

    public void SaveGame() {
        // return right away if data persistence is disabled
        if (disableDataPersistence) {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.playerData == null) {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SaveData(playerData);
        }

        // timestamp the data so we know when it was last saved
        playerData.lastPlayed = DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        dataHandler.Save(playerData, selectedProfileId);
    }
    public void LoadGame() {
        // return right away if data persistence is disabled
        if (disableDataPersistence) {
            return;
        }

        // load any saved data from a file using the data handler
        playerData = dataHandler.Load(selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (playerData == null && initializeDataIfNull) {
            NewGame();
        }

        // if no data can be loaded, don't continue
        if (playerData == null) {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.LoadData(playerData);
        }
    }

    public void OnApplicationQuit() {
        // SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData() {
        return playerData != null;
    }

    public Dictionary<string, PlayerData> GetAllProfilesGameData() {
        return dataHandler.LoadAllProfiles();
    }

    private void InitializeSelectedProfileId() {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId) {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    public void DeleteProfileData(string profileId) {
        // delete the data for this profile id
        dataHandler.Delete(profileId);
        // initialize the selected profile id
        InitializeSelectedProfileId();
        // reload the game so that our data matches the newly selected profile id
        LoadGame();
    }

    public void ChangeSelectedProfileId(string newProfileId) {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }
}
