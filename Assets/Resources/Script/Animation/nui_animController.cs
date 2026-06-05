using UnityEngine;
using UnityEngine.Animations;
public class nui_animController : MonoBehaviour
{
    public Transform monster; 
    public Animator animator;

    private Vector3 lastPosition;


    [SerializeField]
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = monster.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, monster.position, 2f);
        transform.rotation = monster.rotation;

        velocity = 100f * (transform.position - lastPosition);
        lastPosition = transform.position;
        animator.SetFloat("velocityX", velocity.x);
        animator.SetFloat("velocityY", velocity.y);
        animator.SetFloat("velocityZ", velocity.z);
    }
}
