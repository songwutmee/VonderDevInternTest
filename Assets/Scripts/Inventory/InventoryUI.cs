using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject inventoryPanel; 
    public Transform hotbarParent;
    public Transform mainInventoryParent;

    [Header("Selection")]
    public RectTransform selectionHighlight;
    public float lerpSpeed = 15f;
    
    private int selectedSlotIndex = 0;
    private List<InventorySlotUI> allSlotUIs = new List<InventorySlotUI>();

    private void OnEnable()
    {
        // Subscribe to event when enabled
        GameEvents.OnInventoryUpdated += RefreshAllSlots;
    }

    private void OnDisable()
    {
        // To unsubscribe and prevent memory leaks
        GameEvents.OnInventoryUpdated -= RefreshAllSlots;
    }

    private void Start()
    {
        InitializeSlots();
        
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        
        // Refresh
        RefreshAllSlots();
    }

    private void InitializeSlots()
    {
        allSlotUIs.Clear();
        AddSlotsFromParent(hotbarParent);
        AddSlotsFromParent(mainInventoryParent);
    }

    private void AddSlotsFromParent(Transform parent)
    {
        if (parent == null) return;
        for (int i = 0; i < parent.childCount; i++)
        {
            InventorySlotUI slot = parent.GetChild(i).GetComponent<InventorySlotUI>();
            if (slot != null)
            {
                slot.slotIndex = allSlotUIs.Count;
                allSlotUIs.Add(slot);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        HandleHotbarSelection();
        UpdateSelectionHighlight();
    }

    private void HandleHotbarSelection()
    {
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) selectedSlotIndex = i;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            selectedSlotIndex -= (int)Mathf.Sign(scroll);
            if (selectedSlotIndex < 0) selectedSlotIndex = 5;
            if (selectedSlotIndex > 5) selectedSlotIndex = 0;
        }
    }

    private void UpdateSelectionHighlight()
    {
        if (allSlotUIs.Count > selectedSlotIndex && selectedSlotIndex < 6)
        {
            // Defensive check if slot exists
            if (allSlotUIs[selectedSlotIndex] == null) return;

            Vector3 targetPos = allSlotUIs[selectedSlotIndex].transform.position;
            selectionHighlight.position = Vector3.Lerp(selectionHighlight.position, targetPos, Time.deltaTime * lerpSpeed);
        }
    }

    public void RefreshAllSlots()
    {
        for (int i = 0; i < allSlotUIs.Count; i++)
        {
            if (allSlotUIs[i] == null) continue;

            if (i < InventoryManager.Instance.slots.Count)
            {
                var data = InventoryManager.Instance.slots[i];
                allSlotUIs[i].UpdateSlotUI(data.item, data.count);
            }
        }
    }
}