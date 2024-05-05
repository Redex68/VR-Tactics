using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : NetworkBehaviour
{
    [SerializeField] GameEvent gameEnd;
    [SerializeField] PlayerTypeVariable playerType;

    override public void Spawned()
    {
        gameEnd.OnEvent += onGameEnd;
    }

    private void OnDisable()
    {
        gameEnd.OnEvent -= onGameEnd;
    }

    //TODO: Make networked
    void onGameEnd(Component sender, object victor)
    {
        Invoke("loadScene", 5.0f);
        if(playerType.value == PlayerType.VR) RpcNetworkedEventCall((Victor)victor);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.Proxies, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RpcNetworkedEventCall(Victor victor)
    {
        gameEnd.Raise(null, victor);
    }

    void loadScene()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
