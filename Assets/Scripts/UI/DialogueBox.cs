using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour {

    [SerializeField] float writeInterval = 0.03f;

    Image bg;
    TMP_Text textBox;
    GameObject icon;
    
    int pageCount;
    bool finishedWriting = false;
    bool waitingForConfirm = false;


    void Start() {
        bg = transform.GetChild(0).GetComponent<Image>();
        bg.enabled = false;
        textBox = transform.GetChild(1).GetComponent<TMP_Text>();
        icon = transform.GetChild(2).gameObject;
        HideContinueIcon();

        textBox.text = "";
        textBox.enabled = false;
    }

    void Update() {
        if(finishedWriting) {
            if(GameInput.UI.confirm) {
                HideContinueIcon();
                bg.enabled = false;
                textBox.enabled = false;
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

    public void Write(string inputString) {
        GameInput.DisablePlayerControls();
        textBox.enabled = true;
        finishedWriting = false;
        StartCoroutine(RevealTextBox(inputString));
    }

    IEnumerator RevealTextBox(string input) {
        textBox.text = "";
        Color grey = new Color(0.4f, 0.4f, 0.4f, 1);
        Color final = bg.color;
        bg.enabled = true;
        bg.color = Color.Lerp(grey, final, 0);
        yield return new WaitForSeconds(0.2f);
        bg.color = Color.Lerp(grey, final, 0.5f);
        yield return new WaitForSeconds(0.2f);
        bg.color = Color.Lerp(grey, final, 1);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(BeginWrite(input));
    }

    IEnumerator BeginWrite(string inputString) {
        textBox.text = inputString;
        textBox.pageToDisplay = 1;
        textBox.ForceMeshUpdate();
        pageCount = textBox.textInfo.pageCount;

        int totalVisibleCharacters = textBox.textInfo.characterCount;
        textBox.maxVisibleCharacters = 0;

        while(!finishedWriting) {
            // print characters until the end of the page, then pause
            while(textBox.maxVisibleCharacters <= textBox.textInfo.pageInfo[textBox.pageToDisplay-1].lastCharacterIndex) {
                yield return new WaitForSecondsRealtime(writeInterval);
                textBox.maxVisibleCharacters++;
            }
            yield return new WaitForSecondsRealtime(writeInterval * 2);

            ShowContinueIcon();
            // if on the last page, stop writing
            if(textBox.pageToDisplay == pageCount) {
                finishedWriting = true;
                yield break;
            }
            else {
                waitingForConfirm = true;
                yield return new WaitUntil(() => waitingForConfirm == false);
                HideContinueIcon();
                
                // update text info for the next page
                textBox.pageToDisplay++;
            }
        }
    }

    void ShowContinueIcon() {
        icon.SetActive(true);
    }
    void HideContinueIcon() {
        icon.SetActive(false);
    }
}
