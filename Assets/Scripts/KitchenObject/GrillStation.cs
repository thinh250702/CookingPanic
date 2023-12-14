using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillStation : FunctionalObject {

    [SerializeField] private Material[] signalMat;
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    [SerializeField] private List<GrillSlot> leftGroup;
    [SerializeField] private List<GrillSlot> rightGroup;

    [SerializeField] private BinaryStateObject leftKnob;
    [SerializeField] private BinaryStateObject rightKnob;
    [SerializeField] private GameObject leftSignal;
    [SerializeField] private GameObject rightSignal;

    private Animator animator;
    private bool leftKnobOn = false;
    private bool rightKnobOn = false;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        leftKnob.OnObjectInteract += LeftKnob_OnObjectInteract;
        rightKnob.OnObjectInteract += RightKnob_OnObjectInteract;
    }

    private void SetGrillSlotState(List<GrillSlot> group, bool value) {
        foreach (GrillSlot slot in group) {
            slot.isTurnOn = value;
        }
    }

    private void RightKnob_OnObjectInteract(object sender, System.EventArgs e) {
        if (!rightKnobOn) {
            animator.Play("GrillRightKnob_On");
            rightKnobOn = true;
            SetGrillSlotState(rightGroup, true);
            rightSignal.GetComponent<Renderer>().material = signalMat[1];
        } else {
            animator.Play("GrillRightKnob_Off");
            rightKnobOn = false;
            SetGrillSlotState(rightGroup, false);
            rightSignal.GetComponent<Renderer>().material = signalMat[0];
        }
    }

    private void LeftKnob_OnObjectInteract(object sender, System.EventArgs e) {
        if (!leftKnobOn) {
            animator.Play("GrillLeftKnob_On");
            leftKnobOn = true;
            SetGrillSlotState(leftGroup, true);
            leftSignal.GetComponent<Renderer>().material = signalMat[1];
        } else {
            animator.Play("GrillLeftKnob_Off");
            leftKnobOn = false;
            SetGrillSlotState(leftGroup, false);
            leftSignal.GetComponent<Renderer>().material = signalMat[0];
        }
    }

    public bool HasRecipeWithInput(IngredientObjectSO inputObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputObjectSO);
        return fryingRecipeSO != null;
    }

    public FryingRecipeSO GetFryingRecipeSOWithInput(IngredientObjectSO inputObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    public BurningRecipeSO GetBurningRecipeSOWithInput(IngredientObjectSO inputObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
