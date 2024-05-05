using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Object/GameEvent")]
public class GameEvent : ScriptableObject
{
    public delegate void Notify(Component sender, object data);
    public event Notify OnEvent;


    //For legacy reasons (something would probably break without this)
    public void Raise(Component sender, object data)
    {
        OnEvent?.Invoke(sender, data);
    }

    public void SimpleRaise()
    {
        OnEvent?.Invoke(null, null);
    }
}