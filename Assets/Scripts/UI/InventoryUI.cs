using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    
    GameObject bg;
    GameObject layout;
    Inventory playerInventory;
    [SerializeField] RectTransform[] slots;
    public bool open { get; private set; }

    bool toggling = false;

    void Awake() {
        bg = transform.GetChild(0).gameObject;
        layout = transform.GetChild(1).gameObject;
        playerInventory = FindObjectOfType<Player>().GetComponent<Inventory>();
    }

    void Start() {
        open = false;
        bg.SetActive(false);
        layout.SetActive(false);
        Close();
    }

    void Update() {
        if(GameInput.Game.pause) {
            if(!toggling) {
                toggling = true;
                if(open) {
                    ToggleUI(false);
                }
                else {
                    ToggleUI(true);
                }
            }
        }
        else {
            toggling = false;
        }
    }

    void ToggleUI(bool b) {
        open = b;
        bg.SetActive(b);
        layout.SetActive(b);
        Time.timeScale = b ? 0 : 1;
        if(b) {
            GameInput.DisablePlayerControls();
            GameInput.EnableUIControls();
            RefreshInventory();
        }
        else {
            GameInput.EnablePlayerControls();
            GameInput.DisableUIControls();
            Close();
        }
    }

    void RefreshInventory() {
        for(int i = 0; i < playerInventory.size; i++) {
            if(playerInventory.items[i] != null) {
                Image slot = slots[i].GetComponent<Image>();
                slot.color = new Color(1, 1, 1, 1);
                slot.sprite = playerInventory.items[i].item.sprite;
            }
            else {
                Image slot = slots[i].GetComponent<Image>();
                slot.color = new Color(1, 1, 1, 0);
                slot.sprite = null;
            }
            slots[i].gameObject.SetActive(true);
        }
    }
    void Close() {
        for(int i = 0; i < slots.Length; i++) {
            slots[i].gameObject.SetActive(false);
        }
    }

}
