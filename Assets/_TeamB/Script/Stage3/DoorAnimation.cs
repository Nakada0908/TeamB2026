using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationTriggerName = "Door";
    public string targetTag = "Player";

    private void Start()
    {
        animator.Play("Take 001", 0, 0f);
        animator.speed = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            animator.speed = 1f;
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFXSound("DoorOpen");
            }
        }
    }
}
