using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI countText;
    
    private Image slotBackground;
    [HideInInspector] public int slotIndex;
    [HideInInspector] public List<InventorySlot> sourceList;

    private void Awake() { slotBackground = GetComponent<Image>(); }

    public void UpdateSlotUI(InventorySlot data)
    {
        if (data == null || data.item == null || data.count <= 0)
        {
            iconImage.color = new Color(1, 1, 1, 0);
            countText.text = "";
            if (slotBackground != null) slotBackground.color = Color.white;
            return;
        }

        iconImage.sprite = data.item.icon;
        iconImage.color = Color.white;
        countText.text = data.item.maxStackSize > 1 ? data.count.ToString() : "";

        if (slotBackground != null)
        {
            bool isEquipped = InventoryUI.Instance.IsItemEquipped(data.item);
            slotBackground.color = isEquipped ? new Color(1, 0, 0, 0.5f) : Color.white;
        }
    }

    // Detection for Right-Click Double Tap
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (eventData.clickCount == 2)
            {
                // Double Right-Click: Remove 1 item
                InventoryUI.Instance.TrashOneItem(slotIndex, sourceList);
            }
            else
            {
                InventoryUI.Instance.OnSlotRightClick(slotIndex, sourceList);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (sourceList == null || sourceList[slotIndex].item == null) return;
        InventoryUI.Instance.StartDragging(slotIndex, sourceList);
    }

    public void OnDrag(PointerEventData eventData) { InventoryUI.Instance.UpdateDragging(eventData.position); }
    public void OnEndDrag(PointerEventData eventData) { InventoryUI.Instance.StopDragging(eventData); }
}