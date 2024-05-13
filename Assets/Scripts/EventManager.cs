using Fusion;
using UnityEngine;

[RequireComponent(typeof(CustomNetworkSceneManager))]
public class EventManager : NetworkBehaviour
{
    [SerializeField] GameEvent gameEnd;
    [SerializeField] PlayerTypeVariable playerType;

    private CustomNetworkSceneManager sceneManager;
    override public void Spawned()
    {
        gameEnd.OnEvent += onGameEnd;
        sceneManager = GetComponent<CustomNetworkSceneManager>();
    }

    void OnDestroy()
    {
        gameEnd.OnEvent -= onGameEnd;
    }

    //TODO: Make networked
    void onGameEnd(Component sender, object victor)
    {
        if(Runner.IsServer) Invoke("loadLobbyScreen", 5.0f);
        if(playerType.value == PlayerType.VR) RpcNetworkedEventCall((Victor)victor);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RpcNetworkedEventCall(Victor victor)
    {
        gameEnd.Raise(null, victor);
    }

    void loadLobbyScreen()
    {
        sceneManager.OnOpenLobbyScreen(null, null);
    }
}
