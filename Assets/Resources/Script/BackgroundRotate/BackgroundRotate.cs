using UnityEngine;

public class BackgroundRotate : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 5.0f;

    private Vector3 lastPlayerPos;

    private void Start()
    {
        lastPlayerPos = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);

        float moveDelta = player.position.x - lastPlayerPos.x;
        if (moveDelta > 0)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (moveDelta < 0)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        lastPlayerPos = player.position;
    }
}
