using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] TMP_Text buttonEndGameText;

    [SerializeField] GameObject panelEndGame;
    [SerializeField] Button loadGameButton;

    void Start()
    {
        panelEndGame.SetActive(false);

        loadGameButton.gameObject.SetActive(GameSave.LoadGameFromFile() != null);
    }

    private void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnReStartGame += ShowEndGamePanel;
        GameController.OnCountdownChanged += UpdateCountdown;
        GameController.OnTimeChanged += UpdateTime;
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= UpdateScore;
        GameManager.OnReStartGame -= ShowEndGamePanel;
        GameController.OnCountdownChanged -= UpdateCountdown;
        GameController.OnTimeChanged -= UpdateTime;
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    private void UpdateTime(float newTime)
    {
        timeText.text = $"Time: {Mathf.CeilToInt(newTime):0}s";
    }

    private void UpdateCountdown(int countdown)
    {
        countdownText.text = countdown > 0 ? countdown.ToString() : "";
    }

    private void ShowEndGamePanel(bool isWin)
    {
        panelEndGame.SetActive(true);
        buttonEndGameText.text = isWin ? "Next Level" : "Try Again";
    }
}
