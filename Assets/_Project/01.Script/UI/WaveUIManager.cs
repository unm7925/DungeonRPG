using System.Collections;
using TMPro;
using UnityEngine;

namespace _Project._01.Script.UI
{
    public class WaveUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI clearText;
        [SerializeField] private TextMeshProUGUI enemyCountText;
        [SerializeField] private TextMeshProUGUI waveAllClearText;

        public void ShowWaveStart(int waveNumber)
        {
            waveText.gameObject.SetActive(true);
            waveText.text = $"Wave {waveNumber}";

            StartCoroutine(HideTextAfterDelay(waveText, 3f));
        }

        public void ShowWaveClear()
        {
            clearText.gameObject.SetActive(true);
        
            StartCoroutine(HideTextAfterDelay(clearText, 2f));
        }

        public void UpdateEnemyCount(int count)
        {
            enemyCountText.text = $"Enemies: {count}";
        }
    
        public void ShowAllClearWaves()
        {
            waveAllClearText.gameObject.SetActive(true);
            waveText.gameObject.SetActive(false);
            enemyCountText.gameObject.SetActive(false);
            StartCoroutine(HideTextAfterDelay(waveAllClearText, 2f));
        }

        IEnumerator HideTextAfterDelay(TextMeshProUGUI text, float delay)
        {
            yield return new WaitForSeconds(delay);
        
            text.gameObject.SetActive(false);
        }
    }
}
