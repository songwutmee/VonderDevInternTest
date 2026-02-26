using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; 
    public RectTransform selectionHighlight; 
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            public int selectedSlotIndex = 0;

    private void Update()
    {
        // Toggle inventory with E key
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isActive = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(isActive);
        }

        HandleSelection();

        // Right click to use item
        if (Input.GetMouseButtonDown(1)) 
        {
            UseItemAtSelectedSlot();
        }
    }

    private void HandleSelection()
    {
        // Keyboard 1-6 selection
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlotIndex = i;
                UpdateSelectionVisual();
            }
        }

        // Mouse scroll selection
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            selectedSlotIndex -= (int)Mathf.Sign(scroll);
            // Loop index between 0 and 5
            if (selectedSlotIndex < 0) selectedSlotIndex = 5;
            if (selectedSlotIndex > 5) selectedSlotIndex = 0;
            UpdateSelectionVisual();
        }
    }

    private void UpdateSelectionVisual()
    {
        
    }

    private void UseItemAtSelectedSlot()
    {
        InventorySlot slot = InventoryManager.Instance.slots[selectedSlotIndex];
        if (slot.item == null) return;

        switch (slot.item.itemType)
        {
            case ItemType.Usable:
                Debug.Log("Consumed: " + slot.item.itemName);
                InventoryManager.Instance.RemoveItem(slot.item, 1);
                break;
            case ItemType.Equippable:
                Debug.Log("Equipped: " + slot.item.itemName);
                break;
        }
    }
}
