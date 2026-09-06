using UnityEngine;

public class BossEyeRandomHideObj : MonoBehaviour
{
    [SerializeField] private GameObject[] hideObjects;

    private void Start()
    {
        //配列の中からランダムに1つだけ有効化し、他は無効化する
        int randomIndex = Random.Range(0, hideObjects.Length);

        for (int i = 0; i < hideObjects.Length; i++)
        {
            if (i == randomIndex)
            {
                hideObjects[i].SetActive(true);
            }
            else
            {
                hideObjects[i].SetActive(false);
            }
        }
    }
}
