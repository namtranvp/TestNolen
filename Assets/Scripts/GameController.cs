using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public static UnityAction<bool> OnMatch;
    public static UnityAction<Vector2Int> OnSetRowColumn;
    public static UnityAction<int> OnCountdownChanged;
    public static UnityAction<float> OnTimeChanged;
    public static GameController Instance;
    [SerializeField] CardItem cardPrefab;
    [SerializeField] Transform cardContainer;
    [SerializeField] float waitTime = 0.5f;
    [SerializeField] float timeHideCards = 2f;
    [SerializeField] Sprite[] cardSprites;
    private List<CardItem> selectedCards = new List<CardItem>();
    private List<CardItem> allCards = new List<CardItem>();
    private int pairCount = 0;
    private int matchCount = 0;
    private float currentTime = 0f;
    Coroutine countdownCoroutine;
    Coroutine waitAndHideCoroutine;

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
        GameManager.OnReStartGame += ResetCard;
    }

    void OnDisable()
    {
        GameManager.OnReStartGame -= ResetCard;
    }
    public void GenerateCards(Level level)
    {
        // Create a list ID
        currentTime = level.time;
        OnTimeChanged?.Invoke((int)currentTime);

        int numberOfCards = level.cardGrid.x * level.cardGrid.y;
        if (numberOfCards % 2 != 0)
        {
            Debug.LogError("Number of cards must be even!");
            return;
        }
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
            newCard.SetId(idCard, cardSprites[idCard]);
            newCard.onCardSelected += CheckIDCard;

            allCards.Add(newCard);
        }

        OnSetRowColumn?.Invoke(level.cardGrid);
        StartCoroutine(ShowAndFlipAllCards());
    }

    IEnumerator ShowAndFlipAllCards()
    {
        foreach (var card in allCards)
        {
            card.ScaleAnimation();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.3f);
        foreach (var card in allCards)
        {
            card.ShowCard();
        }

        float countdown = timeHideCards;
        while (countdown > 0)
        {
            OnCountdownChanged?.Invoke((int)countdown);
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }
        OnCountdownChanged?.Invoke((int)countdown);

        foreach (var card in allCards)
        {
            card.HideCard();
            card.IsLocked = false;
        }

        allCards.Clear();

        countdownCoroutine = StartCoroutine(CountdowTimer());

    }

    IEnumerator CountdowTimer()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            OnTimeChanged?.Invoke(currentTime);
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

    private void CheckIDCard(CardItem cardItem)
    {
        if (selectedCards.Contains(cardItem) || selectedCards.Count >= 2)
            return;

        selectedCards.Add(cardItem);

        if (selectedCards.Count == 2)
        {
            bool isMatch = selectedCards[0].Id == selectedCards[1].Id;

            OnMatch?.Invoke(isMatch);
            waitAndHideCoroutine = StartCoroutine(WaitAndHideCards(selectedCards[0], selectedCards[1], isMatch));

            selectedCards.Clear();
        }
    }

    private void CheckWin()
    {
        matchCount++;
        if (matchCount >= pairCount)
        {
            Debug.Log("You Win!");
            StopCoroutine(countdownCoroutine);
            GameManager.Instance.WinGame();
        }
    }

    private void ResetCard(bool isWin)
    {
        StopCoroutine(waitAndHideCoroutine);
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        matchCount = 0;
        selectedCards.Clear();
    }
}
