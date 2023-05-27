using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void mainMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void mainScene()
    {
        SceneManager.LoadSceneAsync("Main scene");
    }

    public void exit()
    {
        Application.Quit();
    }
}
