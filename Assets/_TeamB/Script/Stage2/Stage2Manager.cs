using NUnit.Framework.Internal.Commands;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
    public GameObject setDestroyObject1;
    public GameObject setDestroyObject2;

    public GameObject player;

    private void Start()
    {
        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlayBGMSound("Stage2");
        }
    }

    public void AfterDestroy()
    {
        Destroy(setDestroyObject1);
        Destroy(setDestroyObject2);
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFXSound("Destroy");
        }
        //player.GetComponent<PlayerManager_Rigid>().currentStates = EPlayerStates.Stand;
        player.GetComponent<Rigidbody>().useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundManager.instance.PlayBGMSound("Chuubosu");
        }
    }
}
