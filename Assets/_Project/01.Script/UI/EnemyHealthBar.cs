using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
   [SerializeField] private Image fillImage;

   private Transform target;
   private Camera mainCamera;

   [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);


   public void Initialize(Transform enemyTransform)
   {
      target = enemyTransform;
      mainCamera = Camera.main;
   }

   public void UpdateHealth(int currentHP, int maxHP)
   {
      if (fillImage != null)
      {
         fillImage.fillAmount = (float)currentHP / maxHP;
      }
   }

   private void LateUpdate()
   {
      if (target == null)
      {
         Destroy(gameObject);
         return;
      }
      
      transform.position = target.position + offset;

      if (mainCamera != null)
      {
         transform.rotation = mainCamera.transform.rotation;
      }
   }
}
