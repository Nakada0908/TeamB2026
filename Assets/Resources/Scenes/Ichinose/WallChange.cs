using UnityEngine;

public class WallChange : MonoBehaviour
{
    public GameObject[] oldWall;
    public GameObject[] newWall;


    private float totalRotation = 0f;
    private int index = 0;
    public Transform player;
    public Transform boss;

    private float previousAngle;
    private bool passedZero = false;


    void Start()
    {

        for (int i = 0; i < oldWall.Length; i++)
        {
            oldWall[i].SetActive(true);
            newWall[i].SetActive(false);
        }

        Vector3 dir = player.position - boss.position;

        previousAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (previousAngle < 0)
            previousAngle += 360f;

    }

    public void ChangeWall(int index)
    {
        Debug.Log("ChangeWall : " + index);
        Debug.Log("Old : " + oldWall[index].name);
        Debug.Log("New : " + newWall[index].name);

        oldWall[index].SetActive(false);
        newWall[index].SetActive(true);
    }

    void Update()
    {
        Vector3 dir = player.position - boss.position;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360f;

        // ‘OƒtƒŒ[ƒ€‚©‚ç‚Ç‚ê‚¾‚¯‰ñ“]‚µ‚½‚©
        float delta = Mathf.DeltaAngle(previousAngle, angle);

        // ‰ñ“]—Ê‚ð‰ÁŽZ
        totalRotation += delta;

        // Debug—p
        Debug.Log("‰ñ“]—Ê = " + totalRotation);

        // 1Žü‚µ‚½‚ç•Ç‚ðØ‚è‘Ö‚¦
        if (Mathf.Abs(totalRotation) >= 360f)
        {
            if (index < oldWall.Length)
            {
                ChangeWall(index);
                index++;
            }

            // ŽŸ‚Ì1Žü‚ð”‚¦‚é‚½‚ßƒŠƒZƒbƒg
            totalRotation = 0f;
        }

        previousAngle = angle;
    }
}