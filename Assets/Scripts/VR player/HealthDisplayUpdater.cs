using UnityEngine;

public class HealthDisplayUpdater : MonoBehaviour
{
    [SerializeField] FloatEventVariable playerHealth;
    [SerializeField] UnityEngine.UI.Slider slider;

    private TMPro.TMP_Text _text;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth.ValueChanged += OnHealthChange;
        _text = GetComponentInChildren<TMPro.TMP_Text>();
    }

    void OnHealthChange(float oldVal, float newVal)
    {
        slider.value = newVal;
        _text.text = ((int)newVal) + "/250";
    }
}
