using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static UnityAction<int> OnScoreChanged;
    public static UnityAction<bool> OnReStartGame;

    [SerializeField] LevelData levelData;

    int currentLevel = 0;
    private int score = 0;


    #region Unity Methods
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    void OnEnable()
    {
        GameController.OnMatch += MatchFound;
        GameController.OnTimeChanged += GameOver;
    }

    void OnDisable()
    {
        GameController.OnMatch -= MatchFound;
        GameController.OnTimeChanged -= GameOver;
    }
    #endregion

    #region Private Methods

    private void MatchFound(bool isMatch)
    {
        score++;
        OnScoreChanged?.Invoke(score);
    }

    private void GameOver(float time)
    {
        if (time > 0) return;
        AudioManager.instance.PlayLoseSound();
        ResetGame(false);
    }

    private void ResetGame(bool _isWin)
    {
        if (_isWin)
        {
            currentLevel++;
        }

        GameSaveData data = new GameSaveData
        {
            levelCurrent = currentLevel,
            score = score
        };

        GameSave.SaveGameToFile(data);
        OnReStartGame?.Invoke(_isWin);
    }

    #endregion

    #region  Public Methods
    public void OnStartGame()
    {
        levelData = levelData ?? Resources.Load<LevelData>("LevelData");
        Level level = levelData.GetLevel(currentLevel);
        if (level == null)
        {
            Debug.Log("No more levels available. Resetting to level 0.");
            currentLevel = 0;
        }
        GameController.Instance.GenerateCards(level);
    }

    public void OnLoadGame()
    {
        GameSaveData data = GameSave.LoadGameFromFile();
        if (data != null)
        {
            currentLevel = data.levelCurrent;
            score = data.score;

            OnScoreChanged?.Invoke(score);

            OnStartGame();
        }
        else
        {
            Debug.Log("No saved game to load.");
        }
    }

    public void WinGame()
    {
        AudioManager.instance.PlayWinSound();
        ResetGame(true);
    }

    #endregion
}
