using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemToGive;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Only destroy if the inventory actually has room to take it
            bool wasAdded = InventoryManager.Instance.AddItem(itemToGive, amount);
            
            if (wasAdded)
            {
                Debug.Log("Collected: " + itemToGive.itemName);
                Destroy(gameObject);
            }
        }
    }
}