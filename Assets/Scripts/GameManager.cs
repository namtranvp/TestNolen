using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityAction<int> OnScoreChanged;
    public static UnityAction<int> OnTimeChanged;
    public static UnityAction<bool> OnReStartGame;
    private static UnityAction onHideCards;

    [SerializeField] CardItem cardPrefab;
    [SerializeField] Transform cardContainer;
    [SerializeField] int numberOfCards = 12;
    [SerializeField] float timeHideCards = 2f;
    [SerializeField] float waitTime = 0.5f;
    [SerializeField] float timeEndGame = 30f;
    [SerializeField] Sprite[] cardSprites;

    private int score = 0;
    private int pairCount = 0;
    private int matchCount = 0;
    private float currentTime = 0f;

    public bool IsStarted => isStarted;
    private bool isStarted = false;
    private bool isWin = false;
    private Coroutine gameStartCoroutine;

    private List<CardItem> selectedCards = new List<CardItem>();

    #region Unity Methods
    void Awake()
    {
        currentTime = timeEndGame;
    }
    #endregion

    #region Private Methods

    private IEnumerator GameStart()
    {
        yield return new WaitForSeconds(timeHideCards);
        onHideCards?.Invoke();
        isStarted = true;

        while (currentTime > 0 && !isWin)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            OnTimeChanged?.Invoke((int)currentTime);
        }

        GameOver();
    }

    private void GenerateCards()
    {
        // Create a list ID
        pairCount = numberOfCards / 2;
        List<int> ids = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        // Random Index
        for (int i = ids.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = ids[i];
            ids[i] = ids[j];
            ids[j] = temp;
        }

        // Create cards
        for (int i = 0; i < numberOfCards; i++)
        {
            CardItem newCard = Instantiate(cardPrefab, cardContainer);

            int idCard = ids[i];
            newCard.SetId(idCard, this, cardSprites[idCard]);
            onHideCards += newCard.HideCard;
        }
    }

    IEnumerator WaitAndHideCards(CardItem item1, CardItem item2, bool isMatch = false)
    {
        yield return new WaitForSeconds(waitTime);

        if (!isMatch)
        {
            item1.HideCard();
            item2.HideCard();
        }
        else
        {
            item1.DestroyCard();
            item2.DestroyCard();

            CheckWin();
        }
    }

    private void CheckWin()
    {
        if (matchCount >= pairCount)
        {
            isWin = true;
            AudioManager.instance.PlayWinSound();
            ResetGame(true);
        }
    }

    private void GameOver()
    {
        if (isWin) return;
        AudioManager.instance.PlayLoseSound();
        ResetGame(false);
    }

    private void ResetGame(bool _isWin)
    {
        StopCoroutine(gameStartCoroutine);
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        matchCount = 0;
        selectedCards.Clear();
        currentTime = timeEndGame;
        isStarted = false;
        isWin = false;
        onHideCards = null;

        if (_isWin)
        {
            numberOfCards += 2;
        }

        GameSaveData data = new GameSaveData
        {
            numberOfCards = numberOfCards,
            score = score
        };

        GameSave.SaveGameToFile(data);
        OnTimeChanged?.Invoke((int)currentTime);
        OnReStartGame?.Invoke(_isWin);
    }

    #endregion

    #region  Public Methods
    public void OnStartGame()
    {
        GenerateCards();

        gameStartCoroutine = StartCoroutine(GameStart());
    }

    public void OnLoadGame()
    {
        GameSaveData data = GameSave.LoadGameFromFile();
        if (data != null)
        {
            numberOfCards = data.numberOfCards;
            score = data.score;

            OnScoreChanged?.Invoke(score);
            OnTimeChanged?.Invoke((int)currentTime);

            OnStartGame();
        }
        else
        {
            Debug.Log("No saved game to load.");
        }
    }

    public void CheckIDCard(CardItem cardItem)
    {
        if (selectedCards.Contains(cardItem) || selectedCards.Count >= 2)
            return;

        selectedCards.Add(cardItem);

        if (selectedCards.Count == 2)
        {
            bool isMatch = false;
            if (selectedCards[0].Id == selectedCards[1].Id)
            {
                Debug.Log("Match!");
                score++;
                matchCount++;
                OnScoreChanged?.Invoke(score);
                isMatch = true;
                AudioManager.instance.PlayMatchSound();
            }
            else
            {
                Debug.Log("Not match!");
                isMatch = false;
                AudioManager.instance.PlayNoMatchSound();
            }

            StartCoroutine(WaitAndHideCards(selectedCards[0], selectedCards[1], isMatch));

            selectedCards.Clear();
        }
    }
    #endregion
}
