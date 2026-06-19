using UnityEngine;

public class PlayercontrolNEW : MonoBehaviour
{

    public GameObject Boss;
   

    Quaternion targetRotation;
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(Boss.transform.position, -Vector3.up, 0.5f);
            //transform.position += Vector3.up * 0.5f * Time.deltaTime;
   

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(Boss.transform.position, Vector3.up, 0.5f);
            //transform.position -= Vector3.up * 0.5f * Time.deltaTime;
        }
    }
}
