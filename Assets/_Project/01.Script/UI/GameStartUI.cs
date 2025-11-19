
using UnityEngine;

    public class GameStartUI : MonoBehaviour
    {
        [SerializeField] private GameObject startPanel;

        private void Start()
        {
            Time.timeScale = 0f;

            startPanel.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startPanel.SetActive(false);
                Time.timeScale = 1f;
                this.enabled = false;
            }
        }
    }

