using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles events related to the RTS player's menus
public class CustomNetworkSceneManager : NetworkBehaviour
{
    [SerializeField] GameEvent onGameStart;
    [SerializeField] GameEvent onOpenLobbyScreen;
    [SerializeField] GameEvent disconnect;

    void OnEnable()
    {
        onGameStart.OnEvent += OnGameStart;
        onOpenLobbyScreen.OnEvent += OnOpenLobbyScreen;
        disconnect.OnEvent += OpenMainMenu;
    }

    void OnDestroy()
    {
        onGameStart.OnEvent -= OnGameStart;
        onOpenLobbyScreen.OnEvent -= OnOpenLobbyScreen;
        disconnect.OnEvent -= OpenMainMenu;
    }

    #region GameStart
    private void OnGameStart(Component sender, object data)
    {
        RPCOnGameStart();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPCOnGameStart()
    {
        Runner.LoadScene(SceneRef.FromIndex(1), UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    #endregion

    #region OpenLobbyScreen
    public void OnOpenLobbyScreen(Component sender, object data)
    {
        RPCOnOpenMainMenu();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPCOnOpenMainMenu()
    {
        Runner.LoadScene(SceneRef.FromIndex(0), UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    #endregion

    #region OpenMainMenu
    public async void OpenMainMenu(Component sender, object victor)
    {
        await Runner.Shutdown();
        SceneManager.LoadScene("Menu");
    }
    #endregion
}