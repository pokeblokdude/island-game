using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBuilder : MonoBehaviour {

    [SerializeField] GameObject letterPrefab;
    [SerializeField] GameObject textContinueIcon;
    [SerializeField] Vector2 line1Start = new Vector2(8, 20);
    [SerializeField] Vector2 line2Start = new Vector2(8, 8);
    [SerializeField] int endlinePos = 222;
    [SerializeField] float writeInterval = 0.03f;
    [SerializeField] int poolSize = 100;

    Image image;

    Queue<string> words;
    Queue<char> characters;
    Vector2 writePos;
    bool writing;
    bool finishedWriting = false;
    bool waitingForConfirm = false;

    GameObject[] letterPool;
    int poolIndex = 0;

    GameObject icon;

    void Start() {
        image = GetComponent<Image>();
        letterPool = new GameObject[poolSize];

        for(int i = 0; i < poolSize; i++) {
            GameObject g = Instantiate(letterPrefab, transform.position, Quaternion.identity, transform);
            letterPool[i] = g;
            g.GetComponent<Image>().enabled = false;
        }

        icon = Instantiate(textContinueIcon, transform.position, Quaternion.identity, transform);
        icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(230, 6); // move to bottom right corner after spawning
        HideContinueIcon();
    }

    void Update() {
        if(poolIndex > 0 && finishedWriting) {
            if(GameInput.UI.confirm) {
                foreach(GameObject obj in letterPool) {
                    obj.GetComponent<Image>().enabled = false;
                    HideContinueIcon();
                    poolIndex = 0;
                }
                image.enabled = false;
                GameInput.EnablePlayerControls();
                GameInput.DisableUIControls();
            }
        }
        if(!finishedWriting) {
            GameInput.EnableUIControls();
        }
        if(!finishedWriting && waitingForConfirm) {
            if(GameInput.UI.confirm) {
                waitingForConfirm = false;
            }
        }
    }

    public void Build(string inputString) {
        GameInput.DisablePlayerControls();
        finishedWriting = false;
        StartCoroutine(RevealTextBox(inputString));
    }

    IEnumerator RevealTextBox(string input) {
        Color grey = new Color(0.4f, 0.4f, 0.4f, 1);
        Color final = image.color;
        image.enabled = true;
        image.color = Color.Lerp(grey, final, 0);
        yield return new WaitForSeconds(0.2f);
        image.color = Color.Lerp(grey, final, 0.5f);
        yield return new WaitForSeconds(0.2f);
        image.color = Color.Lerp(grey, final, 1);
        yield return new WaitForSeconds(0.2f);
        BeginWrite(input);
    }

    void BeginWrite(string inputString) {
        writePos = line1Start;
        words = new Queue<string>();

        string[] inputWords = inputString.Split(' ');
        int index = 0;
        foreach(string s in inputWords) {
            words.Enqueue(s);
            if(index < inputWords.Length - 1) {
                words.Enqueue(" ");
            }
            index++;
        }

        StartCoroutine(WriteWords(words));
    }

    IEnumerator WriteWords(Queue<string> inputWords) {
        if(inputWords.Count == 0) {
            FinishWriting();
            yield break;
        }

        characters = new Queue<char>();
        string word = inputWords.Dequeue();
        char[] letters = word.ToCharArray();

        int totalWidth = 0;
        bool esc = false;
        foreach(char letter in letters) {
            
            if(esc) {
                if(letter == '\\' || letter == ' ') {
                    esc = false;
                }
            }
            else {
                if(letter == '\\') {
                    esc = true;
                    continue;
                }   
                Sprite s = FontSpriteDictionary.GetLetter(letter);
                totalWidth += (int)s.rect.width + 1;
            }
        }

        // move to a new line if the word width runs off the end
        if(writePos.x + totalWidth > endlinePos) {
            if(writePos.y == line1Start.y) {
                writePos = line2Start;
                if(word == " ") {
                    word = inputWords.Dequeue();
                    letters = word.ToCharArray();
                }
            }
            else if(writePos.y == line2Start.y) {
                waitingForConfirm = true;
                ShowContinueIcon();
                yield return new WaitUntil(() => waitingForConfirm == false);
                foreach(GameObject obj in letterPool) {
                    obj.GetComponent<Image>().enabled = false;
                    HideContinueIcon();
                    poolIndex = 0;
                }
                writePos = line1Start;
                if(word == " ") {
                    word = inputWords.Dequeue();
                    letters = word.ToCharArray();
                }
            }
            else {
                print("Text writing error. Aborting text write.");
                FinishWriting();
                yield break;
            }
        }

        foreach(char letter in letters) {
            characters.Enqueue(letter);
        }

        writing = true;
        StartCoroutine(WriteLetters(characters));
        yield return new WaitUntil(() => writing == false);
        //yield return new WaitForSecondsRealtime(writeInterval);

        if(inputWords.Count != 0) {
            StartCoroutine(WriteWords(inputWords));
        }
        else {
            FinishWriting();
            yield break;
        }

    }

    IEnumerator WriteLetters(Queue<char> q) {
        if(q.Count == 0) {
            writing = false;
            yield break;
        }

        char letter = q.Dequeue();

        // check for meta instructions
        if(letter == '\\') {
            bool esc = false;
            string instr = "";
            while(letter != ' ') {
                letter = q.Dequeue();
                // check if the backslash was meant as an escape char
                if(letter == '\\') {
                    esc = true;
                    break;
                }
                instr += letter.ToString();
            }
            if(instr.Length > 0) {
                instr = instr.Substring(0, instr.Length-1);
            }
            // check commands if not esc char
            if(!esc) {
                switch(instr) {
                    case "n":   // newline
                        if(writePos.y == line1Start.y) {
                            writePos = line2Start;
                            break;
                        }
                        else if(writePos.y == line2Start.y) {
                            waitingForConfirm = true;
                            ShowContinueIcon();
                            yield return new WaitUntil(() => waitingForConfirm == false);
                            foreach(GameObject obj in letterPool) {
                                obj.GetComponent<Image>().enabled = false;
                                HideContinueIcon();
                                poolIndex = 0;
                            }
                            writePos = line1Start;
                            break;
                        }
                        else {
                            print("Text writing error. Aborting text write.");
                            writing = false;
                            yield break;
                        }
                    default: 
                        print("Unknown meta instruction. Continuing write.");
                        break;
                }
                StartCoroutine(WriteLetters(q));
                yield break;
            }
        }

        // check for end of line
        if(writePos.x >= endlinePos) {
            if(writePos.y == line1Start.y) {
                if(letter == ' ') {
                    letter = q.Dequeue();
                }
                writePos = line2Start;
            }
            else if(writePos.y == line2Start.y) {
                waitingForConfirm = true;
                ShowContinueIcon();
                yield return new WaitUntil(() => waitingForConfirm == false);
                foreach(GameObject obj in letterPool) {
                    obj.GetComponent<Image>().enabled = false;
                    HideContinueIcon();
                    poolIndex = 0;
                }
                writePos = line1Start;
            }
            else {
                print("Text writing error. Aborting text write.");
                writing = false;
                yield break;
            }
        }

        GameObject s = letterPool[poolIndex];
        poolIndex++;
        Letter l = s.GetComponent<Letter>();
        l.Init(letter, writePos);

        yield return new WaitForSecondsRealtime(writeInterval);
        l.Show();

        writePos.x += l.GetSize().x + 1;
        if(q.Count != 0) {
            StartCoroutine(WriteLetters(q));
        }
        else {
            writing = false;
            yield break;
        }
    }

    void FinishWriting() {
        finishedWriting = true;
        ShowContinueIcon();
    }

    void ShowContinueIcon() {
        icon.GetComponent<Image>().enabled = true;
    }
    void HideContinueIcon() {
        icon.GetComponent<Image>().enabled = false;
    }
}
