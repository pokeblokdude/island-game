using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FontSpriteDictionary {
    
    public static Dictionary<string, Sprite> d;

    static FontSpriteDictionary() {
        d = new Dictionary<string, Sprite>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/UI/font");
        foreach(Sprite s in sprites) {
            d.Add(s.name, s);
        }
    }

    public static Sprite GetLetter(char letter) {
        string append = "";

        switch(letter) {
            case '.':
                append = "period";
                break;
            case ',':
                append = "comma";
                break;
            case '(':
                append = "open-paren";
                break;
            case ')':
                append = "close-paren";
                break;
            case '*':
                append = "asterisk";
                break;
            case '!':
                append = "exclamation";
                break;
            case '?':
                append = "question-mark";
                break;
            case '@':
                append = "at";
                break;
            case '#':
                append = "pound";
                break;
            case '$':
                append = "dollar";
                break;
            case '%':
                append = "percent";
                break;
            case '^':
                append = "carrot";
                break;
            case '&':
                append = "ampersand";
                break;
            case '-':
                append = "minus";
                break;
            case '=':
                append = "equals";
                break;
            case '_':
                append = "underscore";
                break;
            case '+':
                append = "plus";
                break;
            case '/':
                append = "slash";
                break;
            case '\\':
                append = "backslash";
                break;
            case '\'':
                append = "apos";
                break;
            case '"':
                append = "quote";
                break;
            case ';':
                append = "semicolon";
                break;
            case ':':
                append = "colon";
                break;
            case '`':
                append = "grave";
                break;
            case '~':
                append = "tilda";
                break;
            case '<':
                append = "less-than";
                break;
            case '>':
                append = "greater-than";
                break;
            case '[':
                append = "open-bracket";
                break;
            case ']':
                append = "close-bracket";
                break;
            case '{':
                append = "open-curly";
                break;
            case '}':
                append = "close-curly";
                break;
            case '|':
                append = "pipe";
                break;
            case ' ':
                append = "space";
                break;
            default:
                append = letter.ToString();
                break;
        }
        string spriteName = "font_" + append;
        Sprite s = null;
        try {
            s = d[spriteName];
        }
        catch (KeyNotFoundException) {
            s = d["font_missingChar"];
        }
        return s;
    }
}