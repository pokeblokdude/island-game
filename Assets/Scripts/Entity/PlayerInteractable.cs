using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractable : MonoBehaviour {

    [SerializeField] GameObject tooltipPrefab;
    [SerializeField] InteractionType interactionType;
    
    public UnityEvent OnPlayerInteract;

    GameObject tooltipInstance;
    bool interactionActive;
    bool eventTriggering = false;

    void Update() {
        if(interactionActive) {
            if(GameInput.Player.actionUp && !eventTriggering) {
                eventTriggering = true;
                OnPlayerInteract.Invoke();
            }
            else if(!GameInput.Player.actionUp) {
                eventTriggering = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            interactionActive = true;
            Transform ttAnchor = other.transform.GetChild(0);
            tooltipInstance = Instantiate(
                tooltipPrefab,
                new Vector3(
                    transform.position.x,
                    ttAnchor.position.y - (other.transform.position.y - transform.position.y), 
                    transform.position.z    // ^^ use the distance between player and anchor, regardless of player position
                ),
                Quaternion.identity,
                transform
            );
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Player") {
            interactionActive = false;
            Destroy(tooltipInstance);
        }
    }


}
