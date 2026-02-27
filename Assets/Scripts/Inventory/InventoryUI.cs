using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("UI Containers")]
    public GameObject inventoryRoot;   
    public GameObject inventoryWindow; 
    public GameObject internalCrafting;
    public GameObject stationCrafting; 
    public GameObject chestPanel;      

    [Header("UI Parents")]
    public Transform hotbarParent;
    public Transform bagParent;
    public Transform chestParent;

    [Header("Visuals")]
    public Image dragIcon;
    public RectTransform selectionHighlight;
    public float lerpSpeed = 15f;

    public bool IsAnyMenuOpen => (inventoryRoot != null && inventoryRoot.activeSelf) || (chestPanel != null && chestPanel.activeSelf) || (stationCrafting != null && stationCrafting.activeSelf);

    private List<InventorySlotUI> playerUIs = new List<InventorySlotUI>();
    private List<InventorySlotUI> chestUIs = new List<InventorySlotUI>();
    private int selectedIndex = 0;
    private ItemData equippedItem;
    private int dragIdx = -1;
    private List<InventorySlot> dragSrc;

    private void Awake() { if (Instance == null) Instance = this; }

    private void Start()
    {
        InitializeSlotRegistration();
        GameEvents.OnInventoryUpdated += RefreshUI;
        ForceCloseEverything(); 
    }

    private void OnDestroy() { GameEvents.OnInventoryUpdated -= RefreshUI; }

    private void InitializeSlotRegistration()
    {
        playerUIs.Clear();
        chestUIs.Clear();
        Register(hotbarParent, InventoryManager.Instance.slots, playerUIs);
        Register(bagParent, InventoryManager.Instance.slots, playerUIs);
        foreach (InventorySlotUI ui in chestParent.GetComponentsInChildren<InventorySlotUI>())
        {
            ui.slotIndex = chestUIs.Count;
            chestUIs.Add(ui);
        }
    }

    private void Register(Transform p, List<InventorySlot> d, List<InventorySlotUI> l)
    {
        if (p == null) return;
        foreach (InventorySlotUI ui in p.GetComponentsInChildren<InventorySlotUI>())
        {
            ui.slotIndex = l.Count;
            ui.sourceList = d;
            l.Add(ui);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) TogglePersonalInventory();
        HandleHotbarSelection();
        
        // Use Item Key
        if (Input.GetKeyDown(KeyCode.Q)) TryUseHotbarItem();
    }

    private void TogglePersonalInventory()
    {
        if (inventoryRoot == null) return;
        bool isOpening = !inventoryRoot.activeSelf;
        if (isOpening)
        {
            ForceCloseEverything();
            inventoryRoot.SetActive(true);
            if (inventoryWindow) inventoryWindow.SetActive(true);
            if (internalCrafting) internalCrafting.SetActive(true);
        }
        else ForceCloseEverything();
    }

    public void OpenChest(List<InventorySlot> data)
    {
        ForceCloseEverything();
        if (chestPanel) chestPanel.SetActive(true);
        foreach (var ui in chestUIs) ui.sourceList = data;
        RefreshUI();
    }

    public void OpenStationCrafting()
    {
        ForceCloseEverything();
        if (stationCrafting) stationCrafting.SetActive(true);
        RefreshUI();
    }

    public void CloseExternalMenu() => ForceCloseEverything();

    private void ForceCloseEverything()
    {
        if (inventoryRoot) inventoryRoot.SetActive(false);
        if (inventoryWindow) inventoryWindow.SetActive(false);
        if (internalCrafting) internalCrafting.SetActive(false);
        if (stationCrafting) stationCrafting.SetActive(false);
        if (chestPanel) chestPanel.SetActive(false);
        if (dragIcon) dragIcon.gameObject.SetActive(false);
    }

    // --- Fixed Interaction Logic ---
    private void TryUseHotbarItem()
    {
        if (InventoryManager.Instance == null || InventoryManager.Instance.slots == null) return;
        if (selectedIndex >= InventoryManager.Instance.slots.Count) return;

        InventorySlot slot = InventoryManager.Instance.slots[selectedIndex];
        if (slot == null || slot.item == null) return;

        if (PlayerController.Instance == null)
        {
            Debug.LogError("PlayerController Instance not found in scene!");
            return;
        }

        switch (slot.item.itemType)
        {
            case ItemType.Usable:
                InventoryManager.Instance.RemoveItem(slot.item, 1);
                break;

            case ItemType.Equippable:
                equippedItem = (equippedItem == slot.item) ? null : slot.item;
                break;

            case ItemType.Placeable:
                if (slot.item.worldPrefab != null)
                {
                    Vector3 spawnOffset = PlayerController.Instance.transform.right * 1.5f;
                    Instantiate(slot.item.worldPrefab, PlayerController.Instance.transform.position + spawnOffset, Quaternion.identity);
                    InventoryManager.Instance.RemoveItem(slot.item, 1);
                }
                break;
        }
        RefreshUI();
    }

    public void TrashOneItem(int index, List<InventorySlot> source)
    {
        if (source == null || index >= source.Count) return;
        InventorySlot slot = source[index];
        if (slot.item != null && slot.count > 0)
        {
            slot.count--;
            if (slot.count <= 0)
            {
                if (equippedItem == slot.item) equippedItem = null;
                slot.Clear();
            }
            RefreshUI();
        }
    }

    public void OnSlotRightClick(int index, List<InventorySlot> source) { }

    public void StartDragging(int idx, List<InventorySlot> src)
    {
        if (src == null || idx >= src.Count || src[idx].item == null) return;
        dragIdx = idx; dragSrc = src;
        dragIcon.gameObject.SetActive(true);
        dragIcon.sprite = src[idx].item.icon;
        dragIcon.color = Color.white;
    }

    public void UpdateDragging(Vector2 pos) { if(dragIcon) dragIcon.transform.position = pos; }

    public void StopDragging(PointerEventData data)
    {
        if(dragIcon) dragIcon.gameObject.SetActive(false);
        InventorySlotUI target = GetSlotUnderMouse(data);
        if (target != null) HandleSwap(dragSrc, dragIdx, target.sourceList, target.slotIndex);
        dragIdx = -1;
        RefreshUI();
    }

    private void HandleSwap(List<InventorySlot> sL, int sI, List<InventorySlot> tL, int tI)
    {
        if (sL == null || tL == null || sI >= sL.Count || tI >= tL.Count) return;
        var s = sL[sI]; var t = tL[tI];
        if (t.item == s.item && t.item != null && t.item.maxStackSize > 1)
        {
            int move = Mathf.Min(s.count, t.item.maxStackSize - t.count);
            t.count += move; s.count -= move;
            if (s.count <= 0) s.Clear();
        }
        else
        {
            ItemData tempI = t.item; int tempC = t.count;
            t.item = s.item; t.count = s.count;
            s.item = tempI; s.count = tempC;
        }
    }

    private InventorySlotUI GetSlotUnderMouse(PointerEventData d)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(d, results);
        foreach (var r in results)
        {
            var ui = r.gameObject.GetComponent<InventorySlotUI>();
            if (ui != null) return ui;
        }
        return null;
    }

    public bool IsItemEquipped(ItemData item) => item != null && equippedItem == item;

    public void RefreshUI()
    {
        foreach (var ui in playerUIs) 
            if (ui != null && ui.sourceList != null && ui.slotIndex < ui.sourceList.Count) 
                ui.UpdateSlotUI(ui.sourceList[ui.slotIndex]);

        if (chestPanel && chestPanel.activeSelf) 
            foreach (var ui in chestUIs) 
                if (ui != null && ui.sourceList != null && ui.slotIndex < ui.sourceList.Count) 
                    ui.UpdateSlotUI(ui.sourceList[ui.slotIndex]);
    }

    private void HandleHotbarSelection()
    {
        for (int i = 0; i < 6; i++) if (Input.GetKeyDown(KeyCode.Alpha1 + i)) selectedIndex = i;
        float s = Input.GetAxis("Mouse ScrollWheel");
        if (s != 0)
        {
            selectedIndex -= (int)Mathf.Sign(s);
            if (selectedIndex < 0) selectedIndex = 5;
            if (selectedIndex > 5) selectedIndex = 0;
        }
        if (playerUIs.Count > selectedIndex && selectionHighlight != null)
            selectionHighlight.position = Vector3.Lerp(selectionHighlight.position, playerUIs[selectedIndex].transform.position, Time.deltaTime * lerpSpeed);
    }
}