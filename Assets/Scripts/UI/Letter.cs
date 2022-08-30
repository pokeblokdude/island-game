using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour {

    char inputLetter;
    Image image;
    RectTransform rect;
    Vector2 pos;

    void Awake() {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    void Start() {
        image.sprite = FontSpriteDictionary.GetLetter(inputLetter);
        rect.sizeDelta = new Vector2(image.preferredWidth, image.preferredHeight);
        Vector2 pixelPivot = image.sprite.pivot;
        Vector2 percentPivot = new Vector2(pixelPivot.x / rect.sizeDelta.x, pixelPivot.y / rect.sizeDelta.y);
        rect.pivot = percentPivot;
        rect.anchoredPosition = pos;
    }

    public void Init(char inputLetter, Vector2 pos) {
        this.inputLetter = inputLetter;
        this.pos = pos;
    }

    public Vector2 GetSize() {
        return new Vector2(image.preferredWidth, image.preferredHeight);
    }
}
