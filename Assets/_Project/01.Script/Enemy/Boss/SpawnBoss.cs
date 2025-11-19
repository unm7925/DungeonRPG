using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] private GameObject enemyBoss;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            
            GameObject boss = Instantiate(enemyBoss, transform.position, Quaternion.identity);
            
            GameOverManager.Instance.RegisterBoss(boss.GetComponent<BossAI>());
            gameObject.SetActive(false);
        }
    }
}
