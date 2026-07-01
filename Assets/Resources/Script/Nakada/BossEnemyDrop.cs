using UnityEngine;

public class BossEnemyDrop : MonoBehaviour
{
    [Header("スポーン情報")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        SpawnEnemy();
        this.GetComponent<Collider>().enabled = false;
    }

    private void SpawnEnemy()
    {

    }
}
