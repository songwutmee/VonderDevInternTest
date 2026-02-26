using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;
    public int slotIndex; // Index 0-5 = Hotbar, 6-17 = Main Bag

    // Update slot visuals
    public void UpdateSlot(ItemData item, int count)
    {
        if (item == null || count <= 0)
        {
            iconImage.color = new Color(1, 1, 1, 0); // Transparent
            countText.text = "";
        }
        else
        {
            iconImage.sprite = item.icon;
            iconImage.color = new Color(1, 1, 1, 1); // Visible
            countText.text = count > 1 ? count.ToString() : "";
        }
    }
}