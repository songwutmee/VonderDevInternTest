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

    private List<InventorySlotUI> playerUIs = new List<InventorySlotUI>();
    private List<InventorySlotUI> chestUIs = new List<InventorySlotUI>();
    private int selectedIndex = 0;
    private ItemData equippedItem;
    private int dragIdx = -1;
    private List<InventorySlot> dragSrc;

    private void Awake() { if (Instance == null) Instance = this; }

    private void Start()
    {
        InitSlots();
        GameEvents.OnInventoryUpdated += RefreshUI;
        ForceCloseEverything(); 
    }

    private void OnDestroy() { GameEvents.OnInventoryUpdated -= RefreshUI; }

    private void InitSlots()
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
        if (Input.GetKeyDown(KeyCode.Q)) TryUseHotbarItem();
    }

    private void TogglePersonalInventory()
    {
        bool isOpening = !inventoryRoot.activeSelf;
        if (isOpening)
        {
            ForceCloseEverything();
            inventoryRoot.SetActive(true);
            inventoryWindow.SetActive(true);
            if (internalCrafting) internalCrafting.SetActive(true);
        }
        else
        {
            ForceCloseEverything();
        }
    }

    // Called by StorageChest (E)
    public void OpenChest(List<InventorySlot> data)
    {
        ForceCloseEverything();
        if (chestPanel) chestPanel.SetActive(true);
        foreach (var ui in chestUIs) ui.sourceList = data;
        RefreshUI();
    }

    // Called by CraftingStation (E)
    public void OpenStationCrafting()
    {
        ForceCloseEverything();
        if (stationCrafting) stationCrafting.SetActive(true);
        RefreshUI();
    }

    // (Chest/Station)
    public void CloseExternalMenu() 
    { 
        ForceCloseEverything(); 
    }

    private void ForceCloseEverything()
    {
        if (inventoryRoot) inventoryRoot.SetActive(false);
        if (inventoryWindow) inventoryWindow.SetActive(false);
        if (internalCrafting) internalCrafting.SetActive(false);
        if (stationCrafting) stationCrafting.SetActive(false);
        if (chestPanel) chestPanel.SetActive(false);
        if (dragIcon) dragIcon.gameObject.SetActive(false);
    }

    // --- Interaction ---
    public void StartDragging(int idx, List<InventorySlot> src)
    {
        if (src == null || src[idx].item == null) return;
        dragIdx = idx; dragSrc = src;
        dragIcon.gameObject.SetActive(true);
        dragIcon.sprite = src[idx].item.icon;
    }

    public void UpdateDragging(Vector2 pos) { dragIcon.transform.position = pos; }

    public void StopDragging(PointerEventData data)
    {
        dragIcon.gameObject.SetActive(false);
        InventorySlotUI target = GetSlotUnderMouse(data);
        if (target != null) HandleSwap(dragSrc, dragIdx, target.sourceList, target.slotIndex);
        dragIdx = -1;
        RefreshUI();
    }

    private void HandleSwap(List<InventorySlot> sL, int sI, List<InventorySlot> tL, int tI)
    {
        if (sL == null || tL == null) return;
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

    private void TryUseHotbarItem()
    {
        var slot = InventoryManager.Instance.slots[selectedIndex];
        if (slot == null || slot.item == null) return;

        if (slot.item.itemType == ItemType.Usable) InventoryManager.Instance.RemoveItem(slot.item, 1);
        else if (slot.item.itemType == ItemType.Equippable) equippedItem = (equippedItem == slot.item) ? null : slot.item;
        else if (slot.item.itemType == ItemType.Placeable && slot.item.worldPrefab != null)
        {
            Vector3 pos = PlayerController.Instance.transform.position + (PlayerController.Instance.transform.right * 1.5f);
            Instantiate(slot.item.worldPrefab, pos, Quaternion.identity);
            InventoryManager.Instance.RemoveItem(slot.item, 1);
        }
        RefreshUI();
    }

    public bool IsItemEquipped(ItemData item) => item != null && equippedItem == item;

    public void RefreshUI()
    {
        foreach (var ui in playerUIs) if (ui.sourceList != null) ui.UpdateSlotUI(ui.sourceList[ui.slotIndex]);
        if (chestPanel && chestPanel.activeSelf) 
            foreach (var ui in chestUIs) if (ui.sourceList != null) ui.UpdateSlotUI(ui.sourceList[ui.slotIndex]);
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
        if (playerUIs.Count > selectedIndex)
            selectionHighlight.position = Vector3.Lerp(selectionHighlight.position, playerUIs[selectedIndex].transform.position, Time.deltaTime * lerpSpeed);
    }
}