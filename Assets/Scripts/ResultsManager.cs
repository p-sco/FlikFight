using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    public TextMeshProUGUI winText, P1KOs, P1Falls, P1DmgReceived, P1DmgDealt, P2KOs, P2Falls, P2DmgReceived, P2DmgDealt;

    public string P1CharSelected, P2CharSelected;
    public static Sprite P1Sprite, P2Sprite;
    public Color Player2Color = Color.blue;
    public Color Player1Color = Color.red;

    public bool P1Continue = false;
    public bool P2Continue = false;

    void Start()
    {
        //set win text
        winText.text = MatchStats.Instance.whoWon;

        //set P1 Results
        P1KOs.text = MatchStats.Instance.P1KOs.ToString();
        P1Falls.text = MatchStats.Instance.P1Falls.ToString();
        P1DmgReceived.text = MatchStats.Instance.P1DmgReceived.ToString();
        P1DmgDealt.text = MatchStats.Instance.P1DmgDealt.ToString();

        //set P2 Results
        P2KOs.text = MatchStats.Instance.P2KOs.ToString();
        P2Falls.text = MatchStats.Instance.P2Falls.ToString();
        P2DmgReceived.text = MatchStats.Instance.P2DmgReceived.ToString();
        P2DmgDealt.text = MatchStats.Instance.P2DmgDealt.ToString();

        P1CharSelected = PlayerPrefs.GetString("P1CharSelected");
        P2CharSelected = PlayerPrefs.GetString("P2CharSelected");

        //finding the spritesheets
        Sprite[] P1sprites = Resources.LoadAll<Sprite>("Images/" + P1CharSelected);
        Sprite[] P2sprites = Resources.LoadAll<Sprite>("Images/" + P2CharSelected);

        //change character sprites depending on who won
        if (MatchStats.Instance.whoWon == "Player 1 Wins!") {
            P1Sprite = P1sprites[15];
            P2Sprite = P2sprites[87];
        } else {
            P1Sprite = P1sprites[43];
            P2Sprite = P2sprites[59];
        }

        //setting the character sprites
        GameObject.Find("P1Char").GetComponent<Image>().sprite = P1Sprite;
        GameObject.Find("P1Char").GetComponent<Image>().color = Player1Color;

        GameObject.Find("P2Char").GetComponent<Image>().sprite = P2Sprite;
        GameObject.Find("P2Char").GetComponent<Image>().color = Player2Color;

    }

    public void Continue(int player) {
        if(player == 1) 
            P1Continue = true;
        else if(player == 2)
            P2Continue = true;

        if (P1Continue && P2Continue) {
            Destroy(MatchStats.Instance);
            SceneManager.LoadScene("MenuScene");
        }
    }
}
