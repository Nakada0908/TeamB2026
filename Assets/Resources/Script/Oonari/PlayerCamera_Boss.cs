using UnityEngine;

public class PlayerCamera_Boss : MonoBehaviour
{
    public Transform CenterPoint;
    public Transform target;
    public testmove TestMove;

    Vector3 defalutCameraOffset;    //デフォルトカメラ位置
    Quaternion defalutCameraDir;    //デフォルトカメラ方向
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defalutCameraDir = transform.rotation;
        defalutCameraOffset = transform.position - target.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (TestMove.horizontal > 0)
        {
            transform.RotateAround(CenterPoint.position, Vector3.up, -TestMove.speed * Time.deltaTime);
        }
        else if(TestMove.horizontal < 0)
        {
            transform.RotateAround(CenterPoint.position, Vector3.up, TestMove.speed * Time.deltaTime);
        }
        
    }
}
