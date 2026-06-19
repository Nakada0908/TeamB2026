using UnityEngine;

public class playerctrl : MonoBehaviour
{
    public float movespeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);

        transform.Translate(-move * movespeed * Time.deltaTime);

    }
}
