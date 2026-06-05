using UnityEngine;

public class DissolveScript : MonoBehaviour
{
    public Material dissolveMat;
    public float dissolveSpeed = 1f;

    private float dissolveAmount = 0f;

    void Update()
    {
     
        dissolveAmount += Time.deltaTime * dissolveSpeed;

        if (dissolveAmount < 1f)
            dissolveAmount = 0f;

        dissolveMat.SetFloat("_StartTime", dissolveAmount);
    }
}
