using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrintEvent : MonoBehaviour
{
    [SerializeField] List<GameEvent> Events;
    List<GameEvent.Notify> Handlers = new();


    void Start()
    {
        foreach (var e in Events)
        {
            Handlers.Add((Component sender, object data) => { Debug.Log($"Event called: {e.name}"); });
            e.OnEvent += Handlers.Last();
        }
    }

    private void OnDisable()
    {
        for(int i = 0; i < Handlers.Count; i++)
        {
            Events[i].OnEvent -= Handlers[i];
        }
    }
}
