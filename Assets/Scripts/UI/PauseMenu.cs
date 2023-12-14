using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button recipeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI buttonDescription;

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            GameHandler.Instance.TogglePauseGame();
        });
        recipeButton.onClick.AddListener(() => {

        });
        quitButton.onClick.AddListener(() => {
            // save the game
            DataPersistenceManager.Instance.SaveGame();
            // transition back to the level selection scene
            SceneLoader.Load(SceneLoader.Scene.SelectionScene);
        });
    }

    private void Start() {
        GameHandler.Instance.OnGamePaused += GameHandler_OnGamePaused;
        GameHandler.Instance.OnGameUnpaused += GameHandler_OnGameUnpaused;
        Hide();
    }

    private void GameHandler_OnGameUnpaused(object sender, System.EventArgs e) {
        Cursor.lockState = CursorLockMode.Locked;
        Hide();
    }

    private void GameHandler_OnGamePaused(object sender, System.EventArgs e) {
        Cursor.lockState = CursorLockMode.None;
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
