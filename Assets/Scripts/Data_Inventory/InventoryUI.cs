using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject inventoryPanel;
    public Transform hotbarParent;
    public Transform mainInventoryParent;

    [Header("Selection")]
    public RectTransform selectionHighlight;
    public float lerpSpeed = 15f;
    
    private int selectedSlotIndex = 0;
    private List<InventorySlotUI> allSlots = new List<InventorySlotUI>();

    private void Start()
    {
        // Hotbar Slots (0-5)
        SetupSlotsFromParent(hotbarParent);
        // Bag Slots (6-17)
        SetupSlotsFromParent(mainInventoryParent);

        GameEvents.OnInventoryUpdated += RefreshAllSlots;
        inventoryPanel.SetActive(false);
    }

    private void SetupSlotsFromParent(Transform parent)
    {
        foreach (Transform child in parent)
        {
            InventorySlotUI slotUI = child.GetComponent<InventorySlotUI>();
            if (slotUI != null)
            {
                slotUI.slotIndex = allSlots.Count;
                allSlots.Add(slotUI);
            }
        }
    }

    private void Update()
    {
        // Toggle Inventory Window
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        HandleHotbarInput();
        MoveSelectionHighlight();
    }

    private void HandleHotbarInput()
    {
        // Numbers 1-6
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) selectedSlotIndex = i;
        }

        // Scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            selectedSlotIndex -= (int)Mathf.Sign(scroll);
            if (selectedSlotIndex < 0) selectedSlotIndex = 5;
            if (selectedSlotIndex > 5) selectedSlotIndex = 0;
        }
    }

    private void MoveSelectionHighlight()
    {
        if (allSlots.Count > 0 && selectedSlotIndex < 6)
        {
            // Move red box to current hotbar slot
            Vector3 targetPos = allSlots[selectedSlotIndex].transform.position;
            selectionHighlight.position = Vector3.Lerp(selectionHighlight.position, targetPos, Time.deltaTime * lerpSpeed);
        }
    }

    public void RefreshAllSlots()
    {
        for (int i = 0; i < allSlots.Count; i++)
        {
            // Sync UI with data from InventoryManager
            var dataSlot = InventoryManager.Instance.slots[i];
            allSlots[i].UpdateSlot(dataSlot.item, dataSlot.count);
        }
    }
}