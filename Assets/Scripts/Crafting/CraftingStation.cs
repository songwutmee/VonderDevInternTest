using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CraftingStation : MonoBehaviour
{
    public GameObject stationUIPanel; 
    private bool playerInRange = false;

    private void Update()
    {
        //Press E to open the station UI 
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleStationUI();
        }
    }

    private void ToggleStationUI()
    {
        if (stationUIPanel != null)
        {
            bool isActive = !stationUIPanel.activeSelf;
            stationUIPanel.SetActive(isActive);
            
            Debug.Log("Crafting Station UI: " + (isActive ? "Opened" : "Closed"));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            CraftingManager.Instance.isNearStation = true;
            GameEvents.TriggerInventoryUpdated();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            CraftingManager.Instance.isNearStation = false;
            if (stationUIPanel != null) stationUIPanel.SetActive(false);
            GameEvents.TriggerInventoryUpdated();
        }
    }
}