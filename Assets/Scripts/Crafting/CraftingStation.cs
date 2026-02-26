using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryUI.Instance.stationCrafting.activeSelf)
                InventoryUI.Instance.CloseExternalMenu();
            else
                InventoryUI.Instance.OpenStationCrafting();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            CraftingManager.Instance.isNearStation = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CraftingManager.Instance.isNearStation = false;
            InventoryUI.Instance.CloseExternalMenu();
        }
    }
}