using UnityEngine;

public class CameraRotate: MonoBehaviour
{
    [SerializeField] GameEvent PlayerLoaded;

    void Start()
    {
        PlayerLoaded.OnEvent += DestroyCam;
    }

    private void OnDisable()
    {
        PlayerLoaded.OnEvent -= DestroyCam;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, -7.5f, 0) * Time.deltaTime);
    }

    public void DestroyCam(Component sender, object value)
    {
        GameObject.Destroy(transform.GetChild(0).gameObject);
        GameObject.Destroy(gameObject, 1.0f);
    }
}
