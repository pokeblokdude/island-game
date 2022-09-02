using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    
    [SerializeField] InventoryItem defaultItem;
    public int size { get; private set; } = 20;
    public Item[] items { get; private set; }
    int equippedItem;
    
    void Awake() {
        items = new Item[size];

        for(int i = 0; i < items.Length; i++) {
            //items[i] = new Item(defaultItem, 1);
            items[i] = null;
        }
    }

    void Start() {
        
    }

    void Update() {
        
    }

}

public class Item {
    public InventoryItem item;
    public int count;

    public Item(InventoryItem item, int count) {
        this.item = item;
        this.count = count;
    }
}
