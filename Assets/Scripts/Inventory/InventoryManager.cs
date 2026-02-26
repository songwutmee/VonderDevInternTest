using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;

    public InventorySlot() 
    { 
        item = null; 
        count = 0; 
    }

    public void Clear() 
    { 
        item = null; 
        count = 0; 
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Sizes")]
    public int hotbarSize = 6;
    public int mainInventorySize = 18;
    
    // List of all slots (Hotbar starts at index 0 to 5)
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Fill inventory with empty slots on start
        for (int i = 0; i < hotbarSize + mainInventorySize; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    // Automatically add item to the first available stack or empty slot
    public bool AddItem(ItemData item, int amount = 1)
    {
        // Search for existing stack first (if stackable)
        if (item.maxStackSize > 1)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.count < item.maxStackSize)
                {
                    slot.count += amount;
                    GameEvents.TriggerInventoryUpdated();
                    return true;
                }
            }
        }

        // Search for first empty slot
        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.count = amount;
                GameEvents.TriggerInventoryUpdated();
                return true;
            }
        }
        return false;
    }

    // Check if player has specific amounts of multiple items (for crafting)
    public bool HasIngredients(List<Ingredient> ingredients)
    {
        foreach (var req in ingredients)
        {
            int currentTotal = 0;
            foreach (var slot in slots)
            {
                if (slot.item == req.item) currentTotal += slot.count;
            }
            if (currentTotal < req.amount) return false;
        }
        return true;
    }

    public void RemoveItem(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                int toRemove = Mathf.Min(amount, slots[i].count);
                slots[i].count -= toRemove;
                amount -= toRemove;
                
                if (slots[i].count <= 0) slots[i].Clear();
                if (amount <= 0) break;
            }
        }
        GameEvents.TriggerInventoryUpdated();
    }
}