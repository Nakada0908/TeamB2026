using UnityEngine;

public class NuigurumiGroundChecker : MonoBehaviour
{
    public string groundTag = "Ground";

    private Monster monster;

    private void Awake()
    {
        monster = GetComponent<Monster>();

        if (monster != null)
        {
            monster.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;

        if (monster != null)
        {
            monster.enabled = true;
        }

        Debug.Log("着地");

        Destroy(this);
    }
}
