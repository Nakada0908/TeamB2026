using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct testEventData
{
    public float targetRotation;
    public float targetTime;
    public string eventType;
}

[System.Serializable]
public class testEventDataList
{
    public List<testEventData> events;
}

public class testEventManager : MonoBehaviour
{
    //JSONファイル
    public TextAsset jsonFile;
    //回転を監視する
    public Transform playerTransform;
    //中心点となるボスのTransform
    public Transform bossTransform;

    //前フレームの方向ベクトルを保持する変数に変更
    Vector3 previousDirection;
    float totalRotation;
    float elapsedTime;

    List<testEventData> eventList = new List<testEventData>();
    int currentEventIndex = 0;

    void Start()
    {
        testEventDataList loadedData = JsonUtility.FromJson<testEventDataList>(jsonFile.text);
        eventList = loadedData.events;

        //ボスからプレイヤーへの初期方向ベクトルを計算し保持
        Vector3 initialDir = playerTransform.position - bossTransform.position;
        initialDir.y = 0f;
        previousDirection = initialDir.normalized;
    }

    void Update()
    {
        //bossTransformのnullチェックを追加
        if (currentEventIndex >= eventList.Count || playerTransform == null || bossTransform == null)
        {
            return;
        }

        //現在の方向ベクトルを計算し、前フレームのベクトルとの角度差分を取得
        Vector3 currentDir = playerTransform.position - bossTransform.position;
        currentDir.y = 0f;
        currentDir = currentDir.normalized;
        float deltaAngle = Mathf.Abs(Vector3.SignedAngle(previousDirection, currentDir, Vector3.up));

        totalRotation += deltaAngle;
        elapsedTime += Time.deltaTime;

        //次フレームの計算用に現在の方向ベクトルを保存
        previousDirection = currentDir;

        testEventData currentEvent = eventList[currentEventIndex];

        if (totalRotation >= currentEvent.targetRotation && elapsedTime >= currentEvent.targetTime)
        {
            TriggerEvent(currentEvent.eventType);

            totalRotation -= currentEvent.targetRotation;
            elapsedTime -= currentEvent.targetTime;

            currentEventIndex++;
        }
    }

    void TriggerEvent(string eventType)
    {
        //イベント発生処理を記述する
        Debug.Log("Event:" + eventType);
    }
}