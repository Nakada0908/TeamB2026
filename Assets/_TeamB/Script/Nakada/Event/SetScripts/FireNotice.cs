using UnityEngine;

public class FireNotice : MonoBehaviour
{
    [HideInInspector] public FireManager fireManager;

    private void Start()
    {
        if (fireManager != null)
        {
            fireManager.Fire();
        }

        Destroy(gameObject);
    }
}
