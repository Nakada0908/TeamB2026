using UnityEngine;

public class FallObject : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(TagConsts.Ground))
        {
            rb.isKinematic = true;
        }
    }
}

