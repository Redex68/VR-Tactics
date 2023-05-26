using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public enum Victor { RTSPlayerWin, VRPlayerWin }
    public static UnityEvent<Victor> onGameOver = new UnityEvent<Victor>();

    // Start is called before the first frame update
    void Start()
    {
        onGameOver.AddListener(onGameEnd);
    }

    void onGameEnd(Victor victor)
    {
        Invoke("loadScene", 5.0f);
    }

    void loadScene()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
