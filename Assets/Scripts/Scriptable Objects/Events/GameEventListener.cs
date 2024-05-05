using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent<Component, object> response;

    private void OnEnable()
    {
        gameEvent.OnEvent += (OnEventRaised);
    }

    private void OnDisable()
    {
        gameEvent.OnEvent -= (OnEventRaised);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}