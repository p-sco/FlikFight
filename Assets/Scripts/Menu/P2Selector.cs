using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2Selector : MonoBehaviour
{
    public bool P2Selected = false;
    public static Sprite Char;
    public Color Player2Color = Color.blue;

    public void CharacterSelect(string character) {
        PlayerPrefs.SetString("P2CharSelected", character);
        P2Selected = true;
        GameObject.Find("MenuManager").SendMessage("P2Selected");

        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/" + character);
        Char = sprites[0];
        GameObject.Find("P2Char").GetComponent<Image>().sprite = Char;
        GameObject.Find("P2Char").GetComponent<Image>().color = Player2Color;
        GameObject.Find("P2Char").GetComponent<Image>().enabled = true;

    }
}
