using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Resource, Usable, Equippable, Placeable }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    
    public GameObject worldPrefab; 
    
    public int maxStackSize = 10;
    
    [TextArea] public string description;
}