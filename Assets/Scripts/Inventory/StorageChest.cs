using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChest : MonoBehaviour
{
    public List<InventorySlot> items = new List<InventorySlot>();
    private bool isPlayerNearby = false;

    private void Awake()
    {
        // Initialize 30 slots for the storage chest
        items.Clear();
        for (int i = 0; i < 30; i++) items.Add(new InventorySlot());
    }

    private void Update()
    {
        // Toggle the chest UI when 'E' is pressed within range
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleChest();
        }
    }

    private void ToggleChest()
    {
        if (InventoryUI.Instance == null) return;

        if (InventoryUI.Instance.chestPanel.activeSelf)
        {
            InventoryUI.Instance.CloseExternalMenu();
        }
        else
        {
            InventoryUI.Instance.OpenChest(items);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            // UI closes when walking away 
            InventoryUI.Instance.CloseExternalMenu();
        }
    }
}