using Unity.VisualScripting;
using UnityEngine;

public class Stage1LightOff : MonoBehaviour
{
    [SerializeField]
    private GameObject Directional1;
    [SerializeField]
    private GameObject Directional2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Directional1 && Directional2)
            {
                Directional1.SetActive(false);
                Directional2.SetActive(false);
            }
        }
    }
}
