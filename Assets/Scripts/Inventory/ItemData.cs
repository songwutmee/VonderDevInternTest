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
    public GameObject prefabToSpawn; // For placeable items
    public int maxStackSize = 10;    // Stack items up to this amount
    
    [TextArea] public string description;
}