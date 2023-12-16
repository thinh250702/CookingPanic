using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillStation : FunctionalObject {

    [SerializeField] private List<GrillSlot> leftGroup;
    [SerializeField] private List<GrillSlot> rightGroup;

    [SerializeField] private BinaryStateObject leftKnob;
    [SerializeField] private BinaryStateObject rightKnob;
    [SerializeField] private GameObject leftSignal;
    [SerializeField] private GameObject rightSignal;

    private bool leftKnobOn = false;
    private bool rightKnobOn = false;

    private int rightKnobOnHashed = Animator.StringToHash("GrillRightKnob_On");
    private int rightKnobOffHashed = Animator.StringToHash("GrillRightKnob_Off");
    private int leftKnobOnHashed = Animator.StringToHash("GrillLeftKnob_On");
    private int leftKnobOffHashed = Animator.StringToHash("GrillLeftKnob_Off");

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        leftKnob.OnObjectInteract += LeftKnob_OnObjectInteract;
        rightKnob.OnObjectInteract += RightKnob_OnObjectInteract;
    }

    private void SetGrillSlotState(List<GrillSlot> group, bool value) {
        foreach (GrillSlot slot in group) {
            slot.isTurnedOn = value;
        }
    }

    private void RightKnob_OnObjectInteract(object sender, System.EventArgs e) {
        if (!rightKnobOn) {
            animator.Play(rightKnobOnHashed);
            rightKnobOn = true;
            SetGrillSlotState(rightGroup, true);
            rightSignal.GetComponent<Renderer>().material = signalMat[1];
        } else {
            animator.Play(rightKnobOffHashed);
            rightKnobOn = false;
            SetGrillSlotState(rightGroup, false);
            rightSignal.GetComponent<Renderer>().material = signalMat[0];
        }
    }

    private void LeftKnob_OnObjectInteract(object sender, System.EventArgs e) {
        if (!leftKnobOn) {
            animator.Play(leftKnobOnHashed);
            leftKnobOn = true;
            SetGrillSlotState(leftGroup, true);
            leftSignal.GetComponent<Renderer>().material = signalMat[1];
        } else {
            animator.Play(leftKnobOffHashed);
            leftKnobOn = false;
            SetGrillSlotState(leftGroup, false);
            leftSignal.GetComponent<Renderer>().material = signalMat[0];
        }
    }

    
}
