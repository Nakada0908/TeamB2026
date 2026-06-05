using UnityEngine;

public class DestroyMoveable : MonoBehaviour
{
    public GameObject destroyMoveableObject;

    public void StartDestroy()
    {
        Destroy(destroyMoveableObject);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFXSound("Destroy");
        }
    }

}
