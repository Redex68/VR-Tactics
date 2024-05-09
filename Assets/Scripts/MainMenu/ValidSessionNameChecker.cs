using Fusion;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ValidSessionNameChecker : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField inputField;
    [SerializeField] UnityEngine.UI.Button button;
    [SerializeField] UnityEngine.UI.Button back;
    [SerializeField] GameEvent onSessionStart;
    [SerializeField] GameEvent onSessionStarted;

    void Start()
    {
        button.interactable = false;
    }

    void OnEnable()
    {
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        onSessionStart.OnEvent += OnSessionStart;
        onSessionStarted.OnEvent += OnSessionStarted;
    }

    void OnDisable()
    {
        inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
        onSessionStart.OnEvent -= OnSessionStart;
        onSessionStarted.OnEvent -= OnSessionStarted;
    }

    //Needs to be added to the OnValueChanged event callback in the input field
    public void OnInputFieldValueChanged(string value)
    {
        if (value.Length > 0) button.interactable = true;
        else button.interactable = false;
    }

    private void OnSessionStart(Component sender, object data)
    {
        button.interactable = false;
        back.interactable = false;
        inputField.interactable = false;
    }

    private void OnSessionStarted(Component sender, object data)
    {
        StartGameResult result = (StartGameResult)data;
        if (!result.Ok)
        {
            inputField.interactable = true;
            back.interactable = true;
            if (inputField.text.Length > 0)
                button.interactable = true;
        }
    }
}
