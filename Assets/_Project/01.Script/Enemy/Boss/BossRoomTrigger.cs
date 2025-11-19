
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject bossDoor;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            PlayerAttack attack = other.GetComponent<PlayerAttack>();
            
            SaveData data = new SaveData(playerHealth,inventory, attack,true);
            data.checkpointPosition = other.transform.position;
            SaveManager.Instance.SaveGame(data);
            
            bossDoor.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
