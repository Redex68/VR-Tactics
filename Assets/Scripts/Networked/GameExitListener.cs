using UnityEngine;

public class GameExitListener : MonoBehaviour
{
    [SerializeField] GameEvent onExit;

    void Start()
    {
        onExit.OnEvent += OnExit;
    }

    void OnDestroy()
    {
        onExit.OnEvent -= OnExit;
    }

    void OnExit(Component sender, object data)
    {
        Application.Quit();
    }
}
