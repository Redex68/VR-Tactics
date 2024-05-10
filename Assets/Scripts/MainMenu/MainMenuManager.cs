using Fusion;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Pages")]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject HostScreen;
    [SerializeField] GameObject JoinScreen;
    [SerializeField] GameObject StartScreen;

    [Header("Game Init Elements")]
    [SerializeField] UnityEngine.UI.Button HostGame;
    [SerializeField] TMPro.TMP_InputField HostSessionName;

    [SerializeField] UnityEngine.UI.Button JoinGame;
    [SerializeField] TMPro.TMP_InputField JoinSessionName;

    [SerializeField] GameEvent onSessionStart;
    [SerializeField] GameEvent onSessionStarted;
    [SerializeField] PlayerTypeVariable playerType;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
        HostScreen.SetActive(false);
        JoinScreen.SetActive(false);
        StartScreen.SetActive(false);
    }

    void OnEnable()
    {
        HostGame.onClick.AddListener(OnHostGame);
        JoinGame.onClick.AddListener(OnJoinGame);
        onSessionStarted.OnEvent += OnSessionStarted;
    }

    private void OnDisable()
    {

        HostGame.onClick.RemoveListener(OnHostGame);
        JoinGame.onClick.RemoveListener(OnJoinGame);
        onSessionStarted.OnEvent -= OnSessionStarted;
    }

    public void OnOpenHostScreen()
    {
        MainMenu.SetActive(false);
        HostScreen.SetActive(true);
        JoinScreen.SetActive(false);
    }

    public void OnBack()
    {
        MainMenu.SetActive(true);
        HostScreen.SetActive(false);
        JoinScreen.SetActive(false);
    }

    public void OnOpenJoinScreen()
    {
        MainMenu.SetActive(false);
        HostScreen.SetActive(false);
        JoinScreen.SetActive(true);
    }

    public void OnHostGame()
    {
        string sessionName = HostSessionName.text;
        if(sessionName.Length > 1)
            onSessionStart.Raise(this, new GameStartParams(GameStartParams.GameMode.HOST, sessionName));
    }

    public void OnJoinGame()
    {
        string sessionName = JoinSessionName.text;
        if (sessionName.Length > 1)
            onSessionStart.Raise(this, new GameStartParams(GameStartParams.GameMode.JOIN, sessionName));
    }

    public void OnSessionStarted(Component sender, object data)
    {
        StartGameResult result = (StartGameResult) data;
        if(result.Ok)
        {
            MainMenu.SetActive(false);
            HostScreen.SetActive(false);
            JoinScreen.SetActive(false);

            if (playerType.value == PlayerType.RTS)
            {
                StartScreen.SetActive(true);
            }
        }
    }
}
