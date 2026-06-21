using UnityEngine;

public class FallObject : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            rb.isKinematic = true;
        }

        //プレイヤーに当たったとき動けなくする
        //……なんかうまくいってないかも(´・ω・｀)
        //if (col.gameObject.CompareTag("Player"))
        //{
        //    col.rigidbody.isKinematic = true;
        //}
    }

    private void OnCollisionExit(Collision col)
    {
        //プレイヤーから離れたとき動けるようにする
        //if (col.gameObject.CompareTag("Player"))
        //{
        //    col.rigidbody.isKinematic = false;
        //}
    }
}

