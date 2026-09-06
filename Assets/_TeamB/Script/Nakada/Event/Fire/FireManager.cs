using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] private FireObject[] fireObjects;

    private int currentIndex = 0;
    //セーブしたときに、次が何番目だったか
    private int saveIndex = 0;

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

    //セーブ時に、次に撃つ番号を覚えておく
    public void SaveIndex()
    {
        saveIndex = currentIndex;
    }

    //死んだとき、全部を元の位置に戻して番号も巻き戻す
    public void ResetFire()
    {
        for (int i = 0; i < fireObjects.Length; i++)
        {
            fireObjects[i].ResetToHome();
        }

        currentIndex = saveIndex;
    }
}
