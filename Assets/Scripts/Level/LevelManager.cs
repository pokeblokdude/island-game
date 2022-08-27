using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    [SerializeField] LevelExit defaultEntrance;
    SceneTransitionAnimation anim;
    Player player;

    void Start() {
        anim = FindObjectOfType<SceneTransitionAnimation>();
        player = FindObjectOfType<Player>();
        //SpawnPlayerAtEntrance(SceneTransitionData.exitID);
    }

    public void LoadLevel(SceneReference scene, ExitID exitID) {
        // static reference to SceneTransitionData
        SceneTransitionData.exitID = exitID;
        StartCoroutine(LoadScene(scene));
    }

    IEnumerator LoadScene(SceneReference scene) {
        anim.PlaySceneExit();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
        anim.PlaySceneEnter();
    }
    
    public void ReloadLevel() {
        StartCoroutine(ReloadCurrentScene());
    }

    IEnumerator ReloadCurrentScene() {
        //anim.PlaySceneExit();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //anim.PlaySceneEnter();
    }
    void SpawnPlayerAtEntrance(ExitID exitID) {
        print($"Looking for entrance {SceneTransitionData.exitID}");
        LevelExit correctEntrance = null;
        LevelExit[] exits = FindObjectsOfType<LevelExit>();
        foreach (LevelExit exit in exits) {
            if(exit.exitID == SceneTransitionData.exitID) {
                correctEntrance = exit;
            }
        }
        // use the default spawn position if no correct entrance is found
        if(correctEntrance == null) {
            print("No correct entrance found - falling back to default spawn point.");
            player.transform.position = FindObjectOfType<SpawnPosition>().transform.position;
        }
        else {
            print("Correct entrance found.");
            player.transform.position = correctEntrance.transform.position + 
                new Vector3(SceneTransitionData.playerMoveDir * 2, SceneTransitionData.playerVerticalOffset, 0);
        }
    }
}
