using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PopupMessageUI : MonoBehaviour
{
    public static PopupMessageUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI messageText;

    private CanvasGroup myUIGroup;
    private bool isFaded = false;
    private float waitTime = 2f;


    private void Awake() {
        if (Instance != null) {
            Debug.Log("There is more than one Instance");
        }
        Instance = this;
        messageText.text = "";
    }

    private void Update() {
        if (isFaded) {
            if (myUIGroup.alpha >= 0) {
                myUIGroup.alpha -= Time.deltaTime;
                if (myUIGroup.alpha == 0) {
                    isFaded = false;
                }
            }
        }
    }

    private void Start() {
        myUIGroup = GetComponent<CanvasGroup>();
    }

    public void SetMessage(string msg) {
        myUIGroup.alpha = 1;
        messageText.text = msg;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(waitTime);
        isFaded = true;
    }
}
