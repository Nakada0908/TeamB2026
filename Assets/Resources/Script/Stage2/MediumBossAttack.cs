using UnityEngine;

public class MediumBossAttack : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject mediumBossPrefab;     //ミニボスのプレハブ
    public Transform spawnPoint;            //生成位置

    [Header("落下設定")]
    public float fallSpeed = 10f;           //落下速度

    [Header("床設定")]
    public string floorTag = "fallfloor";   //床のタグ

    private bool spawnedCheck = false;                  //生成済みチェック
    private Rigidbody mediumBossRigidbody;              //ミニボスのRigidbody
    private AudioSource audioSource;

    private void Start()
    {
        if (gameObject.GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }


    // プレイヤーが入ったら障害物生成
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !spawnedCheck)
        {
            GameObject obj = Instantiate(mediumBossPrefab, spawnPoint.position, Quaternion.Euler(90f, 180f, 0f));

            mediumBossRigidbody = obj.GetComponent<Rigidbody>();
            mediumBossRigidbody.useGravity = false;
            mediumBossRigidbody.isKinematic = false;

            spawnedCheck = true;
        }
    }

    // 毎フレーム落下させる
    private void Update()
    {
        if (mediumBossRigidbody != null)
        {
            mediumBossRigidbody.useGravity = true;
        }
    }

    // 落下物が床に当たったら床を落とす
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(floorTag))
        {
            Rigidbody floorRb = collision.gameObject.GetComponent<Rigidbody>();
            if (floorRb != null)
            {
                floorRb.isKinematic = false;
                if (audioSource != null)
                {
                    audioSource.Play();
                    audioSource = null;
                }
            }
        }
    }
}
