using UnityEngine;

public class YagiController : MonoBehaviour
{
    [SerializeField] private Yagi yagi;
    [SerializeField] private EYagiState changeState;
    [SerializeField] private string changeAnimName;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Yagi"))
        {
            yagi.ChangeStatus(changeState, changeAnimName);
            Destroy(gameObject);
        }

    }

}
