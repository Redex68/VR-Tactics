using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIEventSystemManager : MonoBehaviour
{
    [SerializeField] EventManagerType eventManagerType;
    [SerializeField] PlayerTypeVariable playerType;

    MultiplayerEventSystem EventSystem;
    MonoBehaviour InputSystem;

    private enum EventManagerType { VREventManager, RTSEventManager };
    // Start is called before the first frame update
    void Start()
    {
        playerType.ValueChanged += OnPlayerTypeChanged;
        EventSystem = transform.GetComponent<MultiplayerEventSystem>();
        if(eventManagerType == EventManagerType.VREventManager)
            InputSystem = transform.GetComponent<BNG.VRUISystem>();
        else
            InputSystem = transform.GetComponent<InputSystemUIInputModule>();

        EventSystem.enabled = false;
        InputSystem.enabled = false;
        OnPlayerTypeChanged(default, playerType.value);
    }

    void OnDisable()
    {
        playerType.ValueChanged -= OnPlayerTypeChanged;
    }

    void OnPlayerTypeChanged(PlayerType oldVal, PlayerType newVal)
    {
        switch(newVal)
        {
            case PlayerType.VR:
                if(eventManagerType == EventManagerType.VREventManager)
                {
                    EventSystem.enabled = true;
                    InputSystem.enabled = true;
                }
                else
                {
                    EventSystem.enabled = false;
                    InputSystem.enabled = false;
                }
                break;
            case PlayerType.RTS:
                if (eventManagerType == EventManagerType.RTSEventManager)
                {
                    EventSystem.enabled = true;
                    InputSystem.enabled = true;
                }
                else
                {
                    EventSystem.enabled = false;
                    InputSystem.enabled = false;
                }
                break;
        }
    }
}
