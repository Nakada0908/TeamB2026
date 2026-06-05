using UnityEngine;

public class nuigurumiFadeIn : MonoBehaviour
{
    public FadeScript fade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            fade.FadeOut();
        }
    }
}
