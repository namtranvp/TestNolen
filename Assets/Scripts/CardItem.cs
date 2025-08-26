using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardItem : MonoBehaviour, IPointerClickHandler
{
    private int id;
    private GameManager gameManager;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject background;

    public int Id => id;
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlayClickButtonSound();
        if (!gameManager.IsStarted)
            return;

        ShowCard();
        gameManager.CheckIDCard(this);
    }

    public void SetId(int newId, GameManager manager, Sprite sprite)
    {
        id = newId;
        gameManager = manager;
        gameObject.name = "Card_" + id;
        icon.sprite = sprite;
    }

    private void ShowCard()
    {
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f).OnComplete(() =>
        {
            icon.gameObject.SetActive(true);
            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
        });
    }

    public void HideCard()
    {
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f).OnComplete(() =>
        {
            icon.gameObject.SetActive(false);
            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
        });
    }

    public void DestroyCard()
    {
        background.SetActive(false);
    }
}
