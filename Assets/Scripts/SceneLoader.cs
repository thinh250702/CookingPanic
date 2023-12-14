using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public static class SceneLoader
{

    private class LoadingMonoBehaviour : MonoBehaviour { }
    public enum Scene {
        MainMenuScene,
        MainScene,
        SelectionScene,
        LoadingScene
    }

    private static Action OnLoaderCallback;
    private static AsyncOperation loadingOperation;

    public static void Load(Scene scene) {
        // Set the loader callback action to load the target scene
        OnLoaderCallback = () => {
            GameObject loadingGameObject = new GameObject("LoadingGameObject");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        // Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        
    }

    private static IEnumerator LoadSceneAsync(Scene scene) {
        yield return null;

        loadingOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingOperation.isDone) {
            yield return null;
        }
    }

    public static float GetLoadingProgress() {
        if (loadingOperation != null) {
            return loadingOperation.progress;
        } else {
            return 1f;
        }
    }

    public static void LoaderCallback() {
        if (OnLoaderCallback != null) {
            OnLoaderCallback();
            OnLoaderCallback = null;
        }
    }
}
