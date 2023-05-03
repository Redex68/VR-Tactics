using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public void PlayerDied()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
