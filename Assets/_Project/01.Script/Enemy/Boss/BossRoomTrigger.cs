using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject bossDoor;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossDoor.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
