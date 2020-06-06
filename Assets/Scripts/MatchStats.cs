using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStats : MonoBehaviour
{
    public static MatchStats Instance { get; private set; }

    public int P1KOs, P1Falls, P2KOs, P2Falls;
    public float P2DmgReceived, P2DmgDealt, P1DmgReceived, P1DmgDealt;
    public string whoWon;

    private void Awake() {
        //create a singleton
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else { Destroy(gameObject); }
    }
}
