using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private InteractableObject hasProgressObject;
    [SerializeField] private Image barImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;

    private IHasProgress hasProgress;

    private void Start() {
        if (hasProgressObject != null) {
            hasProgress = hasProgressObject.GetComponent<IHasProgress>();
            hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
            barImage.fillAmount = 0f;
            Hide();
        }
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void SetInfo() {
        if (hasProgressObject is StockHolderObject) {
            StockHolderObject stockHolderObject = hasProgressObject as StockHolderObject;
            itemNameText.text = stockHolderObject.GetStockObject().GetStockObjectSO().stockName;
            itemImage.sprite = stockHolderObject.GetStockObject().GetStockObjectSO().sprite;

        } else if (hasProgressObject is CuttingBoard) {
            CuttingBoard cuttingBoard = hasProgressObject as CuttingBoard;
            itemNameText.text = cuttingBoard.GetCuttingObject().GetIngredientObjectSO().ingredientName;
            itemImage.sprite = cuttingBoard.GetCuttingObject().GetIngredientObjectSO().sprite;

        } else if (hasProgressObject is GrillSlot) {
            GrillSlot grillSlot = hasProgressObject as GrillSlot;
            itemNameText.text = grillSlot.GetFryingObject().GetIngredientObjectSO().ingredientName;
            itemImage.sprite = grillSlot.GetFryingObject().GetIngredientObjectSO().sprite;
        }

    }

    private void Show() {
        gameObject.SetActive(true);
        SetInfo();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
