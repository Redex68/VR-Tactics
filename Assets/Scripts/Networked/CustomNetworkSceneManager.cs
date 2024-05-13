using Fusion;
using UnityEngine;

public class CustomNetworkSceneManager : NetworkBehaviour
{
    [SerializeField] GameEvent onGameStart;
    [SerializeField] GameEvent onOpenMainMenu;
    [SerializeField] GameEvent onExitGame;

    void OnEnable()
    {
        onGameStart.OnEvent += OnGameStart;
        onOpenMainMenu.OnEvent += OnOpenMainMenu;
        onExitGame.OnEvent += OnExitGame;
    }

    private void OnDisable()
    {
        onGameStart.OnEvent -= OnGameStart;
        onOpenMainMenu.OnEvent -= OnOpenMainMenu;
        onExitGame.OnEvent -= OnExitGame;
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

    public void OnOpenMainMenu(Component sender, object data)
    {
        RPCOnOpenMainMenu();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPCOnOpenMainMenu()
    {
        Runner.LoadScene(SceneRef.FromIndex(0), UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void OnExitGame(Component sender, object data)
    {
        Application.Quit();
    }
}