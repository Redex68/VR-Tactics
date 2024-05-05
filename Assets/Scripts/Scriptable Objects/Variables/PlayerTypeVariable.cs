using UnityEngine;
public enum PlayerType { RTS, VR };

[CreateAssetMenu(menuName = "Scriptable Object/Player Type Variable")]
public class PlayerTypeVariable : ScriptableVariable<PlayerType>
{
}
