using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePrinter: MonoBehaviour
{
    [SerializeField] GameEvent onSessionStart;
    [SerializeField] GameEvent onSessionStarted;
    [SerializeField] TMPro.TMP_Text textField;
    // Start is called before the first frame update
    void OnEnable()
    {
        onSessionStart.OnEvent += OnSessionStart;
        onSessionStarted.OnEvent += OnSessionStarted;
    }

    private void OnDisable()
    {
        onSessionStart.OnEvent -= OnSessionStart;
        onSessionStarted.OnEvent -= OnSessionStarted;
    }

    private void OnSessionStart(Component sender, object data)
    {
        textField.text = "";
    }

    private void OnSessionStarted(Component sender, object data)
    {
        StartGameResult result = (StartGameResult) data;
        if(!result.Ok)
            textField.text = result.ErrorMessage;
    }
}