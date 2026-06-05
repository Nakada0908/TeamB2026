using UnityEngine;

public class EyeController : MonoBehaviour
{
    [SerializeField] private bool canBlock;
    public GameObject trigger;
    public GameObject lazer;

    public Vector3 moveDistance;

    public float moveSpeed = 2f;

    private Vector3 startAngle;

    void Start()
    {
        startAngle = this.transform.eulerAngles;
    }

    void Update()
    {
        float newX = startAngle.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance.x;
        float newY = startAngle.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance.y;
        float newZ = startAngle.z + Mathf.Sin(Time.time * moveSpeed) * moveDistance.z;
        //transform.position = new Vector3(newX, startPos.y, startPos.z);
        transform.rotation = Quaternion.Euler(newX, newY, newZ);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "moveable")
        {
            if (canBlock)
            {
                if (lazer != null)
                {
                    lazer.SetActive(false);
                }
                trigger.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "moveable")
        {
            if (canBlock)
            {
                {
                    if (lazer != null)
                    {
                        lazer.SetActive(true);
                    }
                    trigger.SetActive(true);
                }
            }
        }
    }
}
