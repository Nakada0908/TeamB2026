using UnityEngine;

public class DebugWarpPosition : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] Transform[] WarpPositions;

    void Update()
    {
        //テンキーに応じてワープ、0がゴール
        for(int i = 0; i < WarpPositions.Length; i++)
        {
            if(Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                player.position = WarpPositions[i].position;
            }
        }
    }
}
