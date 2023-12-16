using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    [Header("Components")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI sfxText;

    private void Awake() {
        musicSlider.onValueChanged.AddListener((value) => {
            MusicManager.Instance.ChangeVolume(value);
            musicText.text = $"{(int)(value * 100)}";
        });
        sfxSlider.onValueChanged.AddListener((value) => {
            // SoundManager.Instance.ChangeVolume(value);
            sfxText.text = $"{(int)(value * 100)}";
        });
        backButton.onClick.AddListener(() => {
            mainMenu.ShowMenu();
            HideMenu();
        });
    }

    private void Start() {
        musicSlider.value = 1;
        sfxSlider.value = 1;
        MusicManager.Instance.ChangeVolume(1f);
        musicText.text = "100";
        // SoundManager.Instance.ChangeVolume(1f);
        sfxText.text = "100";

        HideMenu();
    }

    public void ShowMenu() {
        this.gameObject.SetActive(true);
    }

    public void HideMenu() {
        this.gameObject.SetActive(false);
    }
}
