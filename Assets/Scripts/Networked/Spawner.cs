using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    static private Spawner _instance;

    [SerializeField] private PlayerTypeVariable _playerType;
    [SerializeField] private NetworkPrefabRef _vrPlayerPrefab;
    [SerializeField] private NetworkPrefabRef _networkedManagersPrefab;
    [SerializeField] private GameObject _rtsPlayerPrefab;
    [SerializeField] private GameEvent _onSessionStart;
    [SerializeField] private GameEvent _onSessionStarted;
    [SerializeField] private GameEvent _onPlayerLoaded;

    private GameObject NetworkRunnerObj;
    private NetworkRunner _runner;
    private MyInputActions _playerActionMap;
    private List<PlayerRef> _players = new();
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();

    struct Actions
    {
        //public Vector2 Move;
        //public float Zoom;
        //public bool Shift;
        //public bool MiddleClick;
        //public bool MouseButton0;
        //public bool MouseButton1;

        public void Reset()
        {
            //Shift = false;
            //MiddleClick = false;
            //MouseButton0 = false;
            //MouseButton1 = false;
        }
    }

    private Actions _actions;

    void Awake()
    {

        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    void Start()
    {
        _playerActionMap = new MyInputActions();
        _playerActionMap.UI.Enable();

        _onSessionStart.OnEvent += OnStartGame;
        DontDestroyOnLoad(this);
    }

    void OnDisable()
    {
        _onSessionStart.OnEvent -= OnStartGame;
    }

    private void OnStartGame(Component sender, object data)
    {
        Debug.Log(data.GetType());
        GameStartParams param = (GameStartParams) data;
        Fusion.GameMode mode;
        switch(param.gameMode)
        {
            case GameStartParams.GameMode.HOST:
                mode = GameMode.Host;
                break;
            case GameStartParams.GameMode.JOIN:
                mode = GameMode.Client;
                break;
            default: throw new ArgumentException("Unsupported game mode");
        }
        StartGame(mode, param.sessionName);
    }


    async void StartGame(GameMode mode, string sessionName)
    {
        NetworkRunnerObj = new GameObject("Quarantine");
        NetworkRunnerObj.transform.parent = this.transform;

        _runner = NetworkRunnerObj.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);

        //var physxRunner = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        //physxRunner.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        StartGameResult result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 2,
        });

        if(!result.Ok)
        {
            //Destroy(_runner);
            _onSessionStarted.Raise(this, result);
            return;
        }

        if (mode == GameMode.Host)
        {
            _playerType.value = PlayerType.VR;
        }
        else
        {
            _playerType.value = PlayerType.RTS;
        }
        _onSessionStarted.Raise(this, result);
    }

    IEnumerator DelayedRTSSpawn()
    {
        bool stop = false;
        int count = 0;
        GameObject VRPlayer = null, NetworkedManagers = null;
        while(!stop) 
        {
            yield return new WaitForSeconds(1);
            VRPlayer = GameObject.Find("VR Player");
            NetworkedManagers = GameObject.Find("NetworkedManagers");
            if (VRPlayer != null && NetworkedManagers != null) stop = true;

            if(++count > 10)
            {
                //TODO: Game failed to connect
                yield break;
            }
        }

        var spawnPoint = GameObject.Find("RTS Player Spawn Point");
        spawnPoint.transform.GetPositionAndRotation(out var spawnPosition, out var spawnRotation);
        Instantiate(_rtsPlayerPrefab, spawnPosition, spawnRotation);
        _onPlayerLoaded.Raise(this, null);
    }

    private void SpawnVRPlayer(NetworkRunner runner, PlayerRef player)
    {
        var spawnPoint = GameObject.Find("VR Player Spawn Point");
        var spawnPosition = spawnPoint.transform.position;
        var spawnRotation = spawnPoint.transform.rotation;
        NetworkObject networkPlayerObject;

        if (player != default)
        {
            networkPlayerObject = runner.Spawn(_vrPlayerPrefab, Vector3.zero, Quaternion.identity, player);
            networkPlayerObject.gameObject.GetComponentInChildren<BNG.PlayerTeleport>().TeleportPlayer(spawnPosition, spawnRotation);

            _players.Add(player);
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
        //Case: VR player was spawned once already
        else
        {
            _spawnedCharacters.Remove(player);
            networkPlayerObject = runner.Spawn(_vrPlayerPrefab, Vector3.zero, Quaternion.identity, player);
            networkPlayerObject.gameObject.GetComponentInChildren<BNG.PlayerTeleport>().TeleportPlayer(spawnPosition, spawnRotation);
            _spawnedCharacters.Add(player, networkPlayerObject);
        }

        networkPlayerObject.name = "VR Player";

        var unitCreatorObject = runner.Spawn(_networkedManagersPrefab);
        unitCreatorObject.name = "NetworkedManagers";

        _onPlayerLoaded.Raise(this, null);
    }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    //public void OnMove(InputValue value) { _actions.Move = value.Get<Vector2>(); }
    //public void OnZoom(InputValue value) { _actions.Zoom = value.Get<float>(); }
    //public void OnShift(InputValue value) { _actions.Shift = value.Get<float>() > 0; }
    //public void OnMiddleClick(InputValue value) { _actions.MiddleClick = value.Get<float>() > 0; }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        //data.move = _actions.Move;
        //data.zoom = _actions.Zoom;
        //data.shift = _actions.Shift;
        //data.middleClick = _actions.MiddleClick;
        //data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _actions.MouseButton0);
        //data.buttons.Set(NetworkInputData.MOUSEBUTTON1, _actions.MouseButton1);

        _actions.Reset();

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer && _spawnedCharacters.Count == 0)
            SpawnVRPlayer(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
        _players.Remove(player);
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.IsServer && _spawnedCharacters.Count != 0)
            SpawnVRPlayer(runner, default);
        else if(SceneManager.GetActiveScene().buildIndex == 1 && _playerType.value == PlayerType.RTS)
        {
            StartCoroutine(DelayedRTSSpawn());
        }
    }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}