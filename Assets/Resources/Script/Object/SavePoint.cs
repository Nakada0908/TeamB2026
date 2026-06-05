using Unity.VisualScripting;
using UnityEngine;

public enum ESavePointType
{
    None,
    YagiPoint1,
    YagiPoint2,
    YagiPoint3
}

public class SavePoint : MonoBehaviour
{
    [SerializeField] private ESavePointType savePointType;

    private Vector3 newPos;

    private void Awake()
    {
        newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerSaveManager.instance.SavePlayerPosition(newPos, savePointType);
            Debug.Log("New SavePoint: " + newPos);
            Destroy(gameObject);
        }
    }
}
