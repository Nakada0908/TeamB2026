using UnityEngine;

public class playerRihtHit : MonoBehaviour
{
    private bool hiding;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LightHitBox_Hide"))
        {
            hiding = true;
        }

        if (other.gameObject.CompareTag("LightHitBox"))
        {
            if (!hiding)
            {
                Death();
            }
        }
        if(other.gameObject.CompareTag("Monster"))
        {
            Death();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("LightHitBox"))
        {
            if (!hiding)
            {
                Death();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LightHitBox_Hide"))
        {
            hiding = false;
        }
    }

    private void Death()
    {
        //死んだときの処理を書く
    }
}