public enum Victor { RTS, VR }

public struct GameStartParams
{
    public GameStartParams(GameMode gameMode, string sessionName) { this.gameMode = gameMode; this.sessionName = sessionName; }
    public enum GameMode { HOST, JOIN };
    public GameMode gameMode;
    public string sessionName;

    public override string ToString()
    {
        return $"GameStartParams[{gameMode}, {sessionName}]";
    }
}