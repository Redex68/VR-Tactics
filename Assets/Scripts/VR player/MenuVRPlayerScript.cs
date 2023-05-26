using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuVRPlayerScript : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] public Transform VRPlayerStart;
    [SerializeField] public GameObject enemies;
    private TMPro.TMP_Text health;
    private BNG.Damageable dmg;
    private GameObject currentEnemies;

    void Start() {
        dmg = GetComponent<BNG.Damageable>();
        health = slider.transform.Find("Health").GetComponent<TMPro.TMP_Text>();
        instantiateEnemies();
        updateHealth(dmg.Health);
    }

    public void PlayerDied()
    {
        GetComponent<BNG.PlayerTeleport>().TeleportPlayerToTransform(VRPlayerStart);
        Destroy(currentEnemies);
        Invoke("instantiateEnemies", 2.0f);
        updateHealth(250);
    }

    private void instantiateEnemies()
    {
        currentEnemies = Instantiate(enemies);
        currentEnemies.SetActive(true);
    }

    public void PlayerDamaged(float damage)
    {
        updateHealth(dmg.Health);
    }

    private void updateHealth(float hp)
    {
        slider.value = hp;
        health.text = ((int)hp) + "/250";
    }
}
