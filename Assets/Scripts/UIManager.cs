using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text buttonEndGameText;

    [SerializeField] GameObject panelEndGame;


    void Start()
    {
        panelEndGame.SetActive(false);
    }
    private void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnTimeChanged += UpdateTime;
        GameManager.OnReStartGame += ShowEndGamePanel;
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= UpdateScore;
        GameManager.OnTimeChanged -= UpdateTime;
        GameManager.OnReStartGame -= ShowEndGamePanel;
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    private void UpdateTime(int newTime)
    {
        timeText.text = "Time: " + newTime;
    }

    private void ShowEndGamePanel(bool isWin)
    {
        panelEndGame.SetActive(true);
        scoreText.text = "Score: 0";
        timeText.text = "Time: 30";
        buttonEndGameText.text = isWin ? "Next Level" : "Try Again";
    }
}
