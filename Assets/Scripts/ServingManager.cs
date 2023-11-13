using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingManager : MonoBehaviour {
    public static ServingManager Instance { get; private set; }
    public ContainerObjectSO validObjectSO;

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private CustomerListSO customerListSO;

    private void Awake() {
        Instance = this;
    }

    public RecipeListSO GetRecipeListSO() {
        return recipeListSO;
    }
    public CustomerListSO GetCustomerListSO() {
        return customerListSO;
    }
}
