using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] private GameObject enemyBoss;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Instantiate(enemyBoss, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
