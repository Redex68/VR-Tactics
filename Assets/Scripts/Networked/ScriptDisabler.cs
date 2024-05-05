using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDisabler : MonoBehaviour
{
    [SerializeField] private PlayerTypeVariable playerType;
    [SerializeField] private List<Component> scripts;
    //[SerializeField] private List<Collider> colliders;
    // Start is called before the first frame update
    void Start()
    {
        if(playerType.value != PlayerType.VR)
        {
            foreach (var script in scripts) { GameObject.Destroy(script); }
            //foreach (var collider in colliders) { collider.enabled = false; }
        }

        GameObject.Destroy(this);
    }
}
