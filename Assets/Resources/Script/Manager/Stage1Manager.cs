using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    void Start()
    {
        SoundManager.instance.PlayBGMSound("Stage1");
    }

    // Update is called once per frame
    void Update()
    {
        //// Test
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    SoundManager.instance.PlaySFXSound("SFXTest");
        //}    
    }
}
