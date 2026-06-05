using Unity.VisualScripting;
using UnityEngine;

public class PhantomGirl : MonoBehaviour
{
    public GameObject Girl;
    public GameObject particle;
    private Collider girlCollider;
    Vector3 targetPosition;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            GameObject gen = Instantiate(particle);
            gen.transform.position = transform.position;
            Destroy(gameObject);
        }

    }

}
