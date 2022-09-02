using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="newInventoryItem", menuName="Data/Item/Inventory Item")]
public class InventoryItem : ScriptableObject {
    
    public string itemName = "item";
    public Sprite sprite;
    public Sprite emptySprite;
    public Sprite equippedSprite;
    public int maxStackSize = 10;
    public bool equipable = false;

    [Header("Trading")]
    public bool sellable = true;
    public int shopPrice = 100;

}
