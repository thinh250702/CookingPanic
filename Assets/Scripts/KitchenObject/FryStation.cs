using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryStation : FunctionalObject {

    [SerializeField] private List<FryBasket> fryBasketGroup;

    [SerializeField] private BinaryStateObject fryValve;
    [SerializeField] private BinaryStateObject fryKnob;
    [SerializeField] private GameObject frySignal;

    private bool fryKnobOn = false;
    private bool fryValveOn = false;

    private int fryValveOnHashed = Animator.StringToHash("FryValve_On");
    private int fryValveOffHashed = Animator.StringToHash("FryValve_Off");
    private int fryKnobOnHashed = Animator.StringToHash("FryKnob_On");
    private int fryKnobOffHashed = Animator.StringToHash("FryKnob_Off");
    private int fryOilFilledHashed = Animator.StringToHash("FryOil_Filled");
    private int fryOilEmptyHashed = Animator.StringToHash("FryOil_Empty");

    private void Start() {
        animator = GetComponent<Animator>();
        fryKnob.OnObjectInteract += FryKnob_OnObjectInteract;
        fryValve.OnObjectInteract += FryValve_OnObjectInteract;
    }

    private void SetFryBasketState(List<FryBasket> group, bool value) {
        foreach (FryBasket slot in group) {
            slot.isTurnedOn = value;
        }
    }

    private void FryValve_OnObjectInteract(object sender, System.EventArgs e) {
        if (!fryKnobOn) {
            if (!fryValveOn) {
                animator.Play(fryValveOnHashed);
                animator.Play(fryOilFilledHashed);
                fryValveOn = true;
            } else {
                animator.Play(fryValveOffHashed);
                animator.Play(fryOilEmptyHashed);
                fryValveOn = false;
            }
        } else {
            PopupMessageUI.Instance.SetMessage("You must turn off the knob!");
        }
        
    }

    private void FryKnob_OnObjectInteract(object sender, System.EventArgs e) {
        if (fryValveOn) {
            if (!fryKnobOn) {
                animator.Play(fryKnobOnHashed);
                fryKnobOn = true;
                SetFryBasketState(fryBasketGroup, true);
                frySignal.GetComponent<Renderer>().material = signalMat[1];
            } else {
                animator.Play(fryKnobOffHashed);
                fryKnobOn = false;
                SetFryBasketState(fryBasketGroup, false);
                frySignal.GetComponent<Renderer>().material = signalMat[0];
            }
        } else {
            PopupMessageUI.Instance.SetMessage("You must turn the oil valve first!");
        }
        
    }
}
