using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] private FireObject[] fireObjects;

    private int currentIndex = 0;

    public void Fire()
    {
        if(currentIndex >= fireObjects.Length)
        {
            Debug.Log("FireObjectが足りないよ");
            return;
        }

        FireObject target = fireObjects[currentIndex];

        //表示してから動かす
        target.gameObject.SetActive(true);
        target.StartMove();

        currentIndex++;
    }
}
