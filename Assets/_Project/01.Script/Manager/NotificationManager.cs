using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [SerializeField] private CanvasGroup notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;
    
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float displayDuration = 2f;

    private Queue<string> notificationQueue = new Queue<string>();
    private bool isShowingNotification = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowNotification(string message)
    {
        notificationQueue.Enqueue(message);
        if(!isShowingNotification) StartCoroutine(ProcessQueue());
    }

    private IEnumerator  ProcessQueue()
    {
        while (notificationQueue.Count > 0)
        {
            isShowingNotification = true;
            string message = notificationQueue.Dequeue();
            yield return StartCoroutine(FadeNotification(message));
        }

        isShowingNotification = false;
    }

    private IEnumerator FadeNotification(string message)
    {
        notificationText.text = $"Get Item : " + message;

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            notificationPanel.alpha = elapsed / fadeInDuration;
            yield return null;
        }
        notificationPanel.alpha = 1;
        yield return new WaitForSeconds(displayDuration);

        elapsed = 0;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            notificationPanel.alpha = 1- (elapsed / fadeOutDuration);
            yield return null;
        }
        notificationPanel.alpha = 0;
    }
}
