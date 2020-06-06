using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    bool P1;
    bool P2;
    string selectedStage;
    public GameObject stageButton;
    public GameObject fightButton;
    public GameObject stagePreview;

    public void CharactersSelected() {
        if(P1 && P2)
            stageButton.SetActive(true);
        else
            stageButton.SetActive(false);
    }

    public void P2Selected() {
        P2 = true;
        CharactersSelected();
    }

    public void P1Selected() {
        P1 = true;
        CharactersSelected();
    }

    public void StageSelected(string stage) {
        selectedStage = stage;
        Sprite sprite = Resources.Load<Sprite>("Images/Stages/" + selectedStage);
        stagePreview.GetComponent<Image>().sprite = sprite;
        stagePreview.GetComponent<Image>().enabled = true;
        fightButton.SetActive(true);
    }

    public void StartMatch () {
        SceneManager.LoadScene(selectedStage);
    }
}
