using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingRecipeUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public Button craftButton;
    
    private RecipeData currentRecipe;

    private void Awake()
    {
        if (craftButton != null)
        {
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        }
    }

    public void Setup(RecipeData data)
    {
        currentRecipe = data;
        if (icon != null) icon.sprite = data.resultItem.icon;
        if (nameText != null) nameText.text = data.resultItem.itemName;
        Refresh();
    }

    public void Refresh()
    {
        if (currentRecipe == null) return;

        bool hasMaterials = InventoryManager.Instance.HasIngredients(currentRecipe.ingredients);
        bool stationMet = !currentRecipe.requiresStation || CraftingManager.Instance.isNearStation;

        bool canCraft = hasMaterials && stationMet;
        
        if (craftButton != null)
        {
            craftButton.interactable = canCraft;
            //Dim colkor if can't craft
            craftButton.image.color = canCraft ? Color.white : new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnCraftButtonClicked()
    {
        if (currentRecipe != null && CraftingManager.Instance != null)
        {
            CraftingManager.Instance.Craft(currentRecipe);
        }
    }
}