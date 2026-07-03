using Unity.VisualScripting;
using UnityEngine;

public class CheackDeleteObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DeleteObjects>(out _))
        {
            Destroy(other.gameObject);
        }
    }
}
