using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalSlot : ContainerObject, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    public bool isTurnedOn;

    protected State state;
    protected float fryingTimer;
    protected float burningTimer;
    protected FryingRecipeSO fryingRecipeSO;
    protected BurningRecipeSO burningRecipeSO;
    protected IngredientObject fryingObject;

    private void Start() {
        state = State.Idle;
    }

    protected void InvokeOnProgressChanged(float progressNormalized) {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = progressNormalized
        });
    }

    protected void InvokeOnStateChanged(State state) {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            state = state
        });
    }
}
