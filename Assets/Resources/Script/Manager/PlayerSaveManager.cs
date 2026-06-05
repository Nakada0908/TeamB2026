using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : MonoBehaviour
{
    public static PlayerSaveManager instance;

    [SerializeField]
    private Vector3 playerPosition = new Vector3(0f, 0f, 0f);

    [Header("保存データ")]
    private Vector3 savedPlayerPosition;
    private ESavePointType lastSavePoint = ESavePointType.None;

    [Header("References")]
    [SerializeField] private GameObject yagi;

    private void Awake()
    {
        // Singleton Setting
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void SavePlayerPosition(Vector3 pos,ESavePointType type = ESavePointType.None)
    {
        playerPosition = pos;
        lastSavePoint = type;
    }

    public void LoadPlayer(GameObject player)
    {
        player.transform.position = playerPosition;
        ResetYagiBySavePoint(lastSavePoint);
    }

    public ESavePointType GetSavePointType()
    {
        return lastSavePoint;
    }

    public void ResetYagiBySavePoint(ESavePointType type)
    {
        if (!yagi) return;

        Yagi yagiComp = yagi.GetComponent<Yagi>();

        switch (type)
        {
            case ESavePointType.YagiPoint1:
                yagi.transform.position = new Vector3(46.38f, -24.81f, -1.51f);
                yagi.SetActive(false);
                yagiComp.ChangeStatus(EYagiState.Walk, "Walk");
                break;

            case ESavePointType.YagiPoint2:
                yagiComp.ChangeStatus(EYagiState.Idle, "Idle");
                yagi.transform.position = new Vector3(111.7101f, -24.81f, -1.51f);
                break;

            case ESavePointType.YagiPoint3:
                yagiComp.ChangeStatus(EYagiState.Run, "Run");
                yagi.transform.position = new Vector3(236.4f, -23.9f, 0.3f);
                break;
        }
    }

}
