using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBuilder : MonoBehaviour {

    [SerializeField] GameObject letterPrefab;
    [SerializeField] GameObject textContinueIcon;
    [SerializeField] Vector2 line1Start = new Vector2(8, 20);
    [SerializeField] Vector2 line2Start = new Vector2(8, 8);

    Queue<char> characters;
    Vector2 writePos;
    bool writing;
    bool finishedWriting = false;
    List<GameObject> instantiated;

    void Start() {
        instantiated = new List<GameObject>();
    }

    void Update() {
        if(instantiated.Count > 0 && finishedWriting) {
            if(GameInput.UI.confirm) {
                foreach(GameObject obj in instantiated) {
                    Destroy(obj);
                }
                GameInput.EnablePlayerControls();
                GameInput.DisableUIControls();
            }
        }
        if(!finishedWriting) {
            GameInput.EnableUIControls();
        }
    }

    public void BeginWrite(string inputString = "Hello World.") {
        GameInput.DisablePlayerControls();
        finishedWriting = false;
        writePos = line1Start;
        characters = new Queue<char>();

        char[] letters = inputString.ToCharArray();
        foreach(char c in letters) {
            characters.Enqueue(c);
        }

        StartCoroutine(WriteLetter(characters));
    }

    IEnumerator WriteLetter(Queue<char> q) {
        char letter = q.Dequeue();

        GameObject s = Instantiate(letterPrefab, transform.position, Quaternion.identity, transform);
        instantiated.Add(s);

        Letter l = s.GetComponent<Letter>();
        l.Init(letter, writePos);

        yield return new WaitForSeconds(0.03f);
        
        writePos.x += l.GetSize().x + 1;
        if(q.Count != 0) {
            StartCoroutine(WriteLetter(q));
        }
        else {
            finishedWriting = true;
            GameObject icon = Instantiate(textContinueIcon, transform.position, Quaternion.identity, transform);
            instantiated.Add(icon);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(230, 6); // move to bottom right corner after spawning
            yield break;
        }
    }
}
