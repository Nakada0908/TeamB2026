using UnityEngine;

public class PractureCompleted : MonoBehaviour
{
    [SerializeField]
    private GameObject triggerObject;

    public void ActivePracture()
    {
        triggerObject.SetActive(true);
    }
}
