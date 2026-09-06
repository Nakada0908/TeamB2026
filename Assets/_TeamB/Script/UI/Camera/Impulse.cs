using Unity.Cinemachine;
using UnityEngine;

public class Impulse : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    public float intervalTimer = 1.0f;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= intervalTimer)
        {
            impulseSource.GenerateImpulse();
            timer = 0.0f;
        }
    }
}
