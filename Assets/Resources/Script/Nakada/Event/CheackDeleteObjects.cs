using UnityEngine;

public class CheackDeleteObjects : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject player;

    private void Update()
    {
        transform.position = boss.transform.position - (player.transform.position - boss.transform.position);

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        transform.LookAt(boss.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DeleteObjects>(out _))
        {
            Destroy(other.gameObject);
        }
    }
}
