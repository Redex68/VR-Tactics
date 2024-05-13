using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTransformAtHome : NetworkBehaviour
{
    [Networked] private Vector3Compressed pos { get; set; }
    [Networked] private QuaternionCompressed rot { get; set; }

    public override void Render()
    {
        if(Runner.IsServer)
        {
            pos = transform.position;
            rot = transform.rotation;
        }
        else
        {
            transform.position = pos;
            transform.rotation = rot;
        }
    }
}
