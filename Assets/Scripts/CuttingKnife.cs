using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingKnife : PickableObject
{
    private Animator animator;
    [SerializeField] private CuttingBoard cuttingBoard;
    private void Start() {
        animator = GetComponent<Animator>();
        cuttingBoard.OnCut += CuttingBoard_OnCut;
    }

    private void CuttingBoard_OnCut(object sender, System.EventArgs e) {
        animator.Play("KnifeCut");
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {

        } else {
            NormalPickObject(this, player, Quaternion.Euler(0,180,-90));
        }
    }
}
