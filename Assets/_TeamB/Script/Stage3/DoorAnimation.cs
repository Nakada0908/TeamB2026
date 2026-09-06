using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationTriggerName = "Door";
    public string targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            animator.SetTrigger(animationTriggerName);
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySFXSound("DoorOpen");
            }
        }
    }
}
