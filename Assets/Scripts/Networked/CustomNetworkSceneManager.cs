using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkSceneManager : NetworkBehaviour
{
    [SerializeField] GameEvent onGameStart;

    void OnEnable()
    {
        onGameStart.OnEvent += OnGameStart;
    }

    private void OnDisable()
    {
        onGameStart.OnEvent -= OnGameStart;
    }

    private void OnGameStart(Component sender, object data)
    {
        RPCOnGameStart();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPCOnGameStart()
    {
        Runner.LoadScene(SceneRef.FromIndex(1), UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
