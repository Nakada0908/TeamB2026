using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerTransform;
    //現段階ではプレイヤー追従のみ
    public bool followPlayer = true;

    public static float distance = 8;
    public static Vector3 offset = new Vector3(0, 1, -distance);
    private Vector3 playerPos;
    private Vector3 cameraPos;

    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        playerPos = playerTransform.position;
    }
    void LateUpdate()
    {
        cameraPos = playerPos + offset;
        if (followPlayer)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                cameraPos,
                ref velocity,
                0.3f
                );
        }
    }

    public static void CameraIdle()
    {
        offset = new Vector3(0,3f, -distance);
    }
    public static void CameraLookUp()
    {
        offset = new Vector3 (0, 5f, -distance);
    }
    public static void CameraLookDown()
    {
        offset = new Vector3(0, 1f, -distance);
    }

}
