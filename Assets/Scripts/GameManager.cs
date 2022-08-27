using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("General")]
    [SerializeField] int framerate = 60;
    [SerializeField] bool capFramerate = false;
    [SerializeField][Range(0,1)] float timescale = 1;

    [Header("Level")]
    [SerializeField] LevelManager levelManager;

    InputManager input;
    bool reset = false;

    void Awake() {
        input = new InputManager();
        input.Game.Reset.performed += ctx => {
            if(!reset) {
                levelManager.ReloadLevel();
                reset = true;
            }
        };
        input.Game.Reset.canceled += ctx => {
            reset = false;
        };
        input.Game.Quit.performed += ctx => {
            Application.Quit();
        };
    }

    void Update() {
        if(capFramerate) {
            Application.targetFrameRate = framerate;
        }
        Time.timeScale = timescale;
    }

    void OnEnable() {
        input.Enable();
    }

    void OnDisable() {
        input.Disable();
    }

}
