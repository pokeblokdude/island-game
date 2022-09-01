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

    bool reset = false;

    void Start() {
        Cursor.visible = false;
    }

    void Update() {
        if(capFramerate) {
            Application.targetFrameRate = framerate;
        }
        Time.timeScale = timescale;

        if(GameInput.Game.reset) {
            if(!reset) {
                levelManager.ReloadLevel();
                reset = true;
            }
        }
        else {
            reset = false;
        }

        if(GameInput.Game.quit) {
            Application.Quit();
        }
    }

}
