using UnityEngine;

public class playerRihtHit : MonoBehaviour
{
    private bool hiding;

    #region 敵との接触判定
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagConsts.LightHitBox_Hide))
        {
            hiding = true;
        }

        if (other.gameObject.CompareTag(TagConsts.LightHitBox))
        {
            if (!hiding)
            {
                Death();
            }
        }
        if(other.gameObject.CompareTag(TagConsts.Monster))
        {
            Death();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(TagConsts.LightHitBox))
        {
            if (!hiding)
            {
                Death();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagConsts.LightHitBox_Hide))
        {
            hiding = false;
        }
    }

    private void Death()
    {
        //死んだときの処理を書く
        Debug.Log("死亡～");
    }
    #endregion
}