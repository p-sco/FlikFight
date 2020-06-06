using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P1Selector : MonoBehaviour
{
    public bool P1Selected = false;
    public static Sprite Char;
    public Color Player1Color = Color.red;

    public void CharacterSelect(string character) {
        //set the character
        PlayerPrefs.SetString("P1CharSelected", character);
        //tell menu manager that a character was selected
        P1Selected = true;
        GameObject.Find("MenuManager").SendMessage("P1Selected");


        //set active character sprite to appear on click
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/" + character);
        Char = sprites[0];
        GameObject.Find("P1Char").GetComponent<Image>().sprite = Char;
        GameObject.Find("P1Char").GetComponent<Image>().color = Player1Color;
        GameObject.Find("P1Char").GetComponent<Image>().enabled = true;

        //TODO highlight box of selected character

    }
}
