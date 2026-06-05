using UnityEngine;

public class Door : MonoBehaviour
{
    public bool canOpen = true;

    [SerializeField] private Quaternion from;
    [SerializeField] private Quaternion to;

    private float timeCount = 0f;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (canOpen && (transform.rotation.y <1.0f))
        {
            DoorOpening();
        }
    }

    private void DoorOpening()
    {
        timeCount += Time.deltaTime * 0.1f;
        transform.rotation = Quaternion.Slerp(from, to, timeCount);
        //timeCount += Time.deltaTime;
    }

}
