using System;
using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte MOUSEBUTTON0 = 1;
    public const byte MOUSEBUTTON1 = 2;

    public Vector2 move;
    public float zoom;
    public bool shift;
    public bool middleClick;
    public NetworkButtons buttons;
}
