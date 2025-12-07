using TMPro;
using UnityEngine;     
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;
   [SerializeField] private GameObject gameOverUI;
   [SerializeField] private TextMeshProUGUI timerText;
   [SerializeField] private TextMeshProUGUI killCountText;
   private int killCount;
    private void Update()
    {
        timerText.text = Time.time.ToString("F2") + "s";
    }

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    public void EnableGameOverUI()
    {
        Time.timeScale = .5f;
        gameOverUI.SetActive(true);
    }

    public void UpdateKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}
