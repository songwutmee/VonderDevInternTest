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

    [Header("Configuration")]
    public int hotbarSize = 6;
    public int mainInventorySize = 12;

    [HideInInspector] 
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        slots.Clear();
        for (int i = 0; i < hotbarSize + mainInventorySize; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(ItemData item, int amount = 1)
    {
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

    public bool HasIngredients(List<Ingredient> ingredients)
    {
        foreach (var req in ingredients)
        {
            int total = 0;
            foreach (var slot in slots)
            {
                if (slot.item == req.item) total += slot.count;
            }
            if (total < req.amount) return false;
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