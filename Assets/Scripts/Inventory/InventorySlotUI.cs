using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventorySlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI countText;
    
    [HideInInspector] public int slotIndex;

    public void UpdateSlotUI(ItemData item, int count)
    {
        // Safety check to ensure images/text were assigned
        if (iconImage == null || countText == null) return;

        if (item == null || count <= 0)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0); 
            countText.text = "";
        }
        else
        {
            iconImage.sprite = item.icon;
            iconImage.color = new Color(1, 1, 1, 1);

            // Hide number only for Equippable items
            if (item.itemType == ItemType.Equippable)
            {
                countText.text = "";
            }
            else
            {
                countText.text = count.ToString();
            }
        }
    }
}