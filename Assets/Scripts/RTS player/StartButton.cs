using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadSceneAsync("Main scene");
        GameObject.Find("Waiting").GetComponent<TMPro.TMP_Text>().text = "Loading...";
        GameObject button = GameObject.Find("Start");
        button.GetComponent<UnityEngine.UI.Button>().interactable = false;
        button.GetComponentInChildren<TMPro.TMP_Text>().text = "Loading...";
    }
}
