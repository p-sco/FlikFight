using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameMaster : MonoBehaviour {
    //static instance object tied to class
    public static GameMaster gm { get; private set; }

    //Characters
    public GameObject dwarf;

    //Rules
    public static int P1LifeCount;
    public static int P2LifeCount;

    //char icons
    public static Sprite P1CharIcon;
    public static Sprite P2CharIcon;

    //spawn prefabs and points
    private GameObject player1Prefab;
    private GameObject player2Prefab;
    public Transform p1SpawnPoint;
    public Transform p2SpawnPoint;
    //spawn delay
    public int spawnDelay = 2;

    //stores the selected char to spawn
    string P1CharSelected;
    string P2CharSelected;

    //targets for camera
    CinemachineTargetGroup.Target target;
    CinemachineTargetGroup targetGroup;

    private void Awake() {
        //create a singleton
        if (gm == null) {
            gm = this;
            //DontDestroyOnLoad(gameObject);
        } else { Destroy(gameObject); }

        P1CharSelected = PlayerPrefs.GetString("P1CharSelected");
        P2CharSelected = PlayerPrefs.GetString("P2CharSelected");
        P1LifeCount = 3;
        P2LifeCount = 3;

        P1CharIcon = Resources.Load<Sprite>("Images/" + P1CharSelected + "_Icon");
        P2CharIcon = Resources.Load<Sprite>("Images/" + P2CharSelected + "_Icon");
        GameObject.Find("P1CharIcon").GetComponent<Image>().sprite = P1CharIcon;
        GameObject.Find("P2CharIcon").GetComponent<Image>().sprite = P2CharIcon;

        targetGroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
        target.weight = 1;
        target.radius = 0;

        InstantiateCharacters(true, true);

    }

    private void InstantiateCharacters(bool p1, bool p2) {
        if (p1) {
            switch (P1CharSelected) {
                case "Dwarf":
                    player1Prefab = dwarf;
                    break;
                    //case "char2":
                    //    player1Prefab = char2;
                    //    break;
                    //case "char3":
                    //    player1Prefab = char3;
                    //    break;
            }
            player1Prefab = Instantiate(player1Prefab, p1SpawnPoint.position, p1SpawnPoint.rotation);
            player1Prefab.gameObject.name = "Player1";
            target.target = player1Prefab.transform;

            if (targetGroup.m_Targets[0].target == null) {
                targetGroup.m_Targets.SetValue(target, 0);
            }
        }
        if (p2) {
            switch (P2CharSelected) {
                case "Dwarf":
                    player2Prefab = dwarf;
                    break;
            }
            player2Prefab = Instantiate(player2Prefab, p2SpawnPoint.position, p2SpawnPoint.rotation);
            player2Prefab.gameObject.name = "Player2";
            target.target = player2Prefab.transform;

            if (targetGroup.m_Targets[1].target == null) {
                targetGroup.m_Targets.SetValue(target, 1);
            }
        }
    }


    public IEnumerator RespawnPlayer (string player) {
        yield return new WaitForSeconds(spawnDelay);
        if(player == "Player1") {
            player1Prefab.transform.position = p1SpawnPoint.position;
            player1Prefab.GetComponent<SpriteRenderer>().material.SetColor("_Color", player1Prefab.GetComponent<PlayerController>().PlayerColor);
            player1Prefab.SetActive(true);
            player1Prefab.GetComponent<PlayerController>().OnRespawn();
        } else if (player == "Player2") {
            player2Prefab.transform.position = p2SpawnPoint.position;
            player2Prefab.GetComponent<SpriteRenderer>().material.SetColor("_Color", player2Prefab.GetComponent<PlayerController>().PlayerColor);
            player2Prefab.SetActive(true);
            player2Prefab.GetComponent<PlayerController>().OnRespawn();
        }
    }

    public static void KillPlayer (PlayerController player) {
        string playerName = player.gameObject.name;
        player.gameObject.SetActive(false);
        if(!player.gameObject.activeSelf)
            gm.GameState(playerName);
    }

    public void GameState (string playerName) {
        if(playerName == "Player1") {
            --P1LifeCount;
            GameObject.Find("P1").GetComponent<GameHUD>().SendMessage("LiveCount");
            MatchStats.Instance.P1Falls++;
            MatchStats.Instance.P2KOs++;
        } 
        else if (playerName == "Player2") {
            --P2LifeCount;
            GameObject.Find("P2").GetComponent<GameHUD>().SendMessage("LiveCount");
            MatchStats.Instance.P2Falls++;
            MatchStats.Instance.P1KOs++;
        }

        if (P1LifeCount <= 0) {
            //set results string
            MatchStats.Instance.whoWon = "Player 2 Wins!";

            //Load Match Results screen
            SceneManager.LoadScene("ResultsScreen");
        } else if (P2LifeCount <= 0) {
            MatchStats.Instance.whoWon = "Player 1 Wins!";
            SceneManager.LoadScene("ResultsScreen");
        } else {
            gm.StartCoroutine(gm.RespawnPlayer(playerName));
        }
    }
}
