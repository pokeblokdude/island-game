using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    
    [SerializeField] GameObject heartPF;

    CombatTarget player;
    GameObject[] hearts;
    int maxHealth;

    void Awake() {
        player = FindObjectOfType<Player>().GetComponent<CombatTarget>();
    }

    void Start() {
        GameObject[] c = new GameObject[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) {
            c[i] = transform.GetChild(i).gameObject;
            Destroy(c[i]);
        }

        maxHealth = player.GetCombatStats().maxHealth;
        hearts = new GameObject[maxHealth/2];
        for(int i = 1; i <= maxHealth; i++) {
            if(i % 2 == 0 && i != 0) {
                GameObject obj = Instantiate(heartPF, transform.position, Quaternion.identity, transform);
                RectTransform rect = obj.GetComponent<RectTransform>();
                if(i == 2) {
                    hearts[0] = obj;
                    rect.anchoredPosition = new Vector2(0, 0);
                }
                else if(i > 2) {
                    hearts[(i/2)-1] = obj;
                    rect.anchoredPosition = new Vector2(hearts[(i/2)-2].GetComponent<RectTransform>().anchoredPosition.x + 8, 0);
                }
            }
        }
    }

    void Update() {
        bool recentDamage = player.recentlyDamaged;
        int health = player.health;
        int index = 0;
        for(int i = 1; i <= maxHealth; i++) {
            GameObject heartHalf;
            if(i % 2 == 0) {
                heartHalf = hearts[index].transform.GetChild(1).gameObject;
                index++;
            }
            else {
                heartHalf = hearts[index].transform.GetChild(0).gameObject;
            }

            heartHalf.SetActive(i > player.health ? false : true);
        }

        // damage flash
        foreach(GameObject heart in hearts) {
            heart.transform.GetChild(2).gameObject.SetActive(recentDamage);
        }   
    }
}
