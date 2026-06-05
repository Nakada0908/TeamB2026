using UnityEngine;

public class YagiSpawner : MonoBehaviour
{
    [SerializeField] private GameObject yagi;

    // 初めて山羊を生成するのはfirst,Goalの前に呼び出しのはsecondです。
    [SerializeField] private bool first = false;
    [SerializeField] private bool second = false;

    private Vector3 secondPosition = new Vector3(236.4f, -23.9f, 0.3f);

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (first)
            {
                if(yagi)
                {
                    yagi.SetActive(true);
                }
            }

            if(second)
            {
                if(yagi)
                {
                    yagi.GetComponent<Yagi>().ChangeStatus(EYagiState.Run, "Run");
                    yagi.transform.localPosition = secondPosition;
                }
            }
        }
    }

}
