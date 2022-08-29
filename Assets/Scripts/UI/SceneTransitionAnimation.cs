using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransitionAnimation : MonoBehaviour {
    
    [SerializeField] RectTransform rect;
    [SerializeField] public float animationTime { get; private set; } = 0.5f;

    void Start() {
        PlaySceneEnter();
    }

    public void PlaySceneEnter() {
        rect.DOAnchorPos(Vector2.left * 800, animationTime).SetEase(Ease.OutCubic);
    }

    public void PlaySceneExit() {
        rect.anchoredPosition = new Vector2(800, 0);
        rect.DOAnchorPos(Vector2.zero, animationTime).SetEase(Ease.OutCubic);
    }
    
}
