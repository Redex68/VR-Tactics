using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
//using Fusion.Addons.Physics;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public enum PlayerType { RTS, VR };
    public static PlayerType playerType = PlayerType.VR;

    [SerializeField] private NetworkPrefabRef _vrPlayerPrefab;
    [SerializeField] private NetworkPrefabRef _unitManagerPrefab;
    [SerializeField] private GameObject _rtsPlayerPrefab;
    private NetworkRunner _runner;
    private MyInputActions _playerActionMap;
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

    async void StartGame(GameMode mode)
    {
        _playerActionMap = new MyInputActions();
        _playerActionMap.UI.Enable();

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        //var physxRunner = gameObject.AddComponent<RunnerSimulatePhysics3D>();
        //physxRunner.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 2
        });

        if (mode == GameMode.Host)
        {
            playerType = PlayerType.VR;
        }
        else
        {
            playerType = PlayerType.RTS;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);

        GameObject.Find("VR Player Networked(Clone)").name = "VR Player";
        GameObject.Find("UnitManager(Clone)").name = "UnitManager";
        GameObject player = Instantiate(_rtsPlayerPrefab, new Vector3(-20, 20, -30), Quaternion.identity);
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host")) StartGame(GameMode.Host);
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join")) StartGame(GameMode.Client);
        }
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
        {
            var spawnPosition = new Vector3(0, 3, 0);
            var networkPlayerObject = runner.Spawn(_vrPlayerPrefab, spawnPosition, Quaternion.identity, player);
            networkPlayerObject.name = "VR Player";

            var unitCreatorObject = runner.Spawn(_unitManagerPrefab);
            unitCreatorObject.name = "UnitManager";

            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
}