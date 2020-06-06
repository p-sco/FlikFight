using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Image[] livesImgs; //0-2, left-right
    public string playerName;
    public int lives;

    void Update()
    {
        healthText.text = GameObject.Find(playerName).GetComponent<PlayerController>().health.ToString() + "%";
        
    }

    public void LiveCount() {
        if (playerName == "Player1") {
            lives = GameMaster.P1LifeCount;
        } else if (playerName == "Player2") {
            lives = GameMaster.P2LifeCount;
        }

        switch (lives) {
            case 3:
                foreach (Image img in livesImgs) {
                    img.gameObject.SetActive(true);
                }
                break;
            case 2:
                livesImgs[0].gameObject.SetActive(true);
                livesImgs[1].gameObject.SetActive(true);
                livesImgs[2].gameObject.SetActive(false);
                break;
            case 1:
                livesImgs[0].gameObject.SetActive(true);
                livesImgs[1].gameObject.SetActive(false);
                livesImgs[2].gameObject.SetActive(false);
                break;
            case 0:
                livesImgs[0].gameObject.SetActive(false);
                livesImgs[1].gameObject.SetActive(false);
                livesImgs[2].gameObject.SetActive(false);
                break;
        }
    }
}
