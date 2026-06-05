using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    //second秒後にオブジェクトを削除
    public float second;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, second);
    }
}
