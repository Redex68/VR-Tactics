using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class SessionNameSetter : MonoBehaviour
{
    TMPro.TMP_Text sessionName;

    void Awake()
    {
        sessionName = GetComponent<TMPro.TMP_Text>();
    }

    void OnEnable()
    {
        sessionName.text = $"Session name: {Spawner.GetSessionName()}";
    }
}
