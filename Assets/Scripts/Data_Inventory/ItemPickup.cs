using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemToGive;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When player walks over the item
        if (other.CompareTag("Player"))
        {
            bool success = InventoryManager.Instance.AddItem(itemToGive, amount);
            if (success)
            {
                // Item picked up successfully
                Debug.Log("Picked up: " + itemToGive.itemName);
                Destroy(gameObject); 
            }
        }
    }
}
