using UnityEngine;

public class DebugPlayerSpeedChange : MonoBehaviour
{
    [SerializeField] private PlayerManager_Boss playerSpeed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            //スピードを戻す
            playerSpeed.speed = 10f;
        }

        if(Input.GetKeyDown(KeyCode.Keypad8))
        {
            //スピードを上げる
            playerSpeed.speed += 40f;
        }
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            //スピードを下げる
            playerSpeed.speed -= 40f;
        }
    }
}