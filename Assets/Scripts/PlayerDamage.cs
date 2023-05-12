using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class PlayerDamage : MonoBehaviour
{
    [SerializeField] public Slider slider;
    private TMPro.TMP_Text health;
    private BNG.Damageable dmg;

    void Start() {
        dmg = GetComponent<BNG.Damageable>();
        health = slider.transform.Find("Health").GetComponent<TMPro.TMP_Text>();
        updateHealth();
    }

    public void PlayerDied()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void PlayerDamaged(float damage)
    {
        updateHealth();
    }

    private void updateHealth()
    {
        slider.value = dmg.Health;
        health.text = ((int)dmg.Health) + "/250";
    }
}
