using UnityEngine;

public class WallChange : MonoBehaviour
{
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
    }

    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(index < oldWall.Length)
            {
                ChangeWall(index);
                index++;
            }
        }
    }
}
