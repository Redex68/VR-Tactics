using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTransformAtHome : NetworkBehaviour
{
    [Networked] private Vector3 pos { get; set; }
    [Networked] private Quaternion rot { get; set; }

    public override void Render()
    {
        if(Spawner.playerType != Spawner.PlayerType.VR)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
        else
        {
            pos = transform.position;
            rot = transform.rotation;
        }
    }
}
