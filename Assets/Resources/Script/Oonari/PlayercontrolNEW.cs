using UnityEngine;

public class PlayercontrolNEW : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Boss;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(Boss.transform.position, -Vector3.up, 0.5f);
            transform.position += Vector3.up * 0.5f * Time.deltaTime;
            
            
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(Boss.transform.position, Vector3.up, 0.5f);
            transform.position -= Vector3.up * 0.5f * Time.deltaTime;
        }
    }
}
