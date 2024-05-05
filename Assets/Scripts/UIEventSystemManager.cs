using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class UIEventSystemManager : MonoBehaviour
{
    [SerializeField] EventManagerType eventManagerType;
    [SerializeField] GameEvent playerTypeSelected;
    [SerializeField] PlayerTypeVariable playerType;

    private enum EventManagerType { VREventManager, RTSEventManager };
    // Start is called before the first frame update
    void Start()
    {
        playerTypeSelected.OnEvent += OnPlayerTypeChanged;
    }

    void OnDisable()
    {
        playerTypeSelected.OnEvent -= OnPlayerTypeChanged;
    }

    void OnPlayerTypeChanged(Component sender, object data)
    {
        MultiplayerEventSystem EventSystem;
        MonoBehaviour InputSystem;
        switch(eventManagerType)
        {
            case EventManagerType.VREventManager:
                EventSystem = transform.GetComponent<MultiplayerEventSystem>();
                if(playerType.value == PlayerType.RTS)
                {
                    InputSystem = transform.GetComponent<BNG.VRUISystem>();
                    EventSystem.enabled = false;
                    InputSystem.enabled = false;
                }
                //else
                //{
                //    EventSystem.playerRoot = GameObject.Find("RTSPlayer");
                //}
                break;
            case EventManagerType.RTSEventManager:
                EventSystem = transform.GetComponent<MultiplayerEventSystem>();
                if(playerType.value == PlayerType.VR)
                {
                    InputSystem = transform.GetComponent<InputSystemUIInputModule>();
                    EventSystem.enabled = false;
                    InputSystem.enabled = false;
                }
                //else
                //{
                //    EventSystem.playerRoot = GameObject.Find("VR Player");
                //}
                break;
        }
    }
}
