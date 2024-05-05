using UnityEngine;


public abstract class ScriptableEventVariable<T> : ScriptableObject
{
    [SerializeField] T defaultValue;
    [Tooltip("Whether the default value will be set each time the game is reloaded")]
    [SerializeField] bool useDefault;
    public T value {
        get
        {
            return _value;
        }
        set
        {
            T old = value;
            _value = value;
            ValueChanged?.Invoke(old, value);
        }
    }
    private T _value;

    public delegate void Notify(T oldVal, T newVal);
    public event Notify ValueChanged;

    void OnEnable()
    {
        ValueChanged = null;
        if (useDefault) value = defaultValue;
    }
}