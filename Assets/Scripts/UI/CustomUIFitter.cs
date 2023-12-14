using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class CustomUIFitter : MonoBehaviour
{
    public enum Type {
        Root,
        Children
    }

    public Type type;

    public RectTransform container;
    private void Awake() {

    }
    private void Start() {
        
    }
    void Update()
    {
        SetContainerHeight();
    }

    private void SetContainerHeight() {
        float spacing = 0;
        if (container.TryGetComponent<VerticalLayoutGroup>(out VerticalLayoutGroup verticalLayoutGroup)) {
            spacing = verticalLayoutGroup.spacing;
        } 
        float height = 0;
        float childNum = 0;
        foreach (Transform child in container) {
            if (child.gameObject.activeSelf == true) {
                childNum++;
                height = height + child.GetComponent<RectTransform>().rect.height;
            }
        }
        //Debug.Log(container.name + "" + height + " " + spacing + " " + childNum);
        if (type == Type.Root) {
            container.sizeDelta = new Vector2(container.sizeDelta.x, height + spacing * (childNum - 1 + 2));
        } else {
            container.sizeDelta = new Vector2(container.sizeDelta.x, height + spacing * (childNum - 1));
        }
        
    }
}
