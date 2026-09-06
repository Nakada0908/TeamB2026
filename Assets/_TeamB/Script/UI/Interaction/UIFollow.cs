using UnityEngine;


public class UIFollow : MonoBehaviour
{
    public Transform targetTransform;//the object that need follow
    //{ get; set; }can't use because cancel the interaction
    public Vector3 offset = new Vector3(0, 2.0f, 0);
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform != null)
        {
            transform.position = targetTransform.position + offset;//show at the object's position + offset
            transform.forward = mainCamera.transform.forward;
        }
    }
}
