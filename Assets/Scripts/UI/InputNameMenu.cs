using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputNameMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button continueButton;

    [SerializeField] private TMP_InputField inputField;

    private void Awake() {
        continueButton.onClick.AddListener(() => {
            DataPersistenceManager.Instance.NewGame(inputField.text);
            SaveGameAndLoadScene();
        });
    }

    private void Start() {
        HideMenu();
    }

    private void SaveGameAndLoadScene() {
        // save the game anytime before loading a new scene
        DataPersistenceManager.Instance.SaveGame();
        // load the scene
        SceneLoader.Load(SceneLoader.Scene.SelectionScene);
    }

    public void ShowMenu() {
        this.gameObject.SetActive(true);
    }

    public void HideMenu() {
        this.gameObject.SetActive(false);
    }
}
