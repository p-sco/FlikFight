using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    public Image[] LivesImgs; //0-2, left-right
    public string PlayerName;
    public int Lives;

    void Update()
    {
        HealthText.text = GameObject.Find(PlayerName).GetComponent<PlayerController>().Health.ToString() + "%";
    }

    public void LiveCount() {
        if (PlayerName == "Player1") {
            Lives = GameMaster.P1LifeCount;
        } else if (PlayerName == "Player2") {
            Lives = GameMaster.P2LifeCount;
        }

        switch (Lives) {
            case 3:
                foreach (Image img in LivesImgs) {
                    img.gameObject.SetActive(true);
                }
                break;
            case 2:
                LivesImgs[0].gameObject.SetActive(true);
                LivesImgs[1].gameObject.SetActive(true);
                LivesImgs[2].gameObject.SetActive(false);
                break;
            case 1:
                LivesImgs[0].gameObject.SetActive(true);
                LivesImgs[1].gameObject.SetActive(false);
                LivesImgs[2].gameObject.SetActive(false);
                break;
            case 0:
                LivesImgs[0].gameObject.SetActive(false);
                LivesImgs[1].gameObject.SetActive(false);
                LivesImgs[2].gameObject.SetActive(false);
                break;
        }
    }
}
