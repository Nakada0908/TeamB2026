using UnityEngine;

public class WallChange : MonoBehaviour
{
    private bool[] changed;
    public GameObject[] oldWall;
    public GameObject[] newWall;
    public Transform player;
    public Transform boss;
    public void ChangeWall(int index)
    {
        oldWall[index].SetActive(false);
        newWall[index].SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < newWall.Length; i++)
        {
            newWall[i].SetActive(false);
            oldWall[i].SetActive(true);
        }
        //changed = new bool[oldWall.Length];

        //for (int i = 0; i < newWall.Length; i++)
        //{
        //    newWall[i].SetActive(false);
        //    oldWall[i].SetActive(true);
        //    changed[i] = false;
        //}
    }

    int index = 0;
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if(index < oldWall.Length)
        //    {
        //        ChangeWall(index);
        //        index++;
        //    }
        //}
        // プレイヤーからボスへの方向
        Vector3 dir = player.position - boss.position;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360f;

        int currentIndex = Mathf.FloorToInt(angle / 22.5f);
        currentIndex = Mathf.Clamp(currentIndex, 0, oldWall.Length - 1);

        if (!changed[currentIndex])
        {
            ChangeWall(currentIndex);
            changed[currentIndex] = true;
        }

    }
}
