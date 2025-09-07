using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CardItem : MonoBehaviour, IPointerClickHandler
{
    public UnityAction<CardItem> onCardSelected;
    private int id;
    private bool isLocked = false;
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    [SerializeField] private Image icon;
    [SerializeField] private GameObject background;

    public int Id => id;


    void OnEnable()
    {
        isLocked = true;
        icon.gameObject.SetActive(false);
        transform.localRotation = Quaternion.Euler(Vector3.forward * 90f);
        transform.localScale = Vector3.zero;
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlayClickButtonSound();
        if (isLocked)
            return;

        ShowCard();
        onCardSelected?.Invoke(this);
    }

    public void SetId(int newId, Sprite sprite)
    {
        id = newId;
        gameObject.name = "Card_" + id;
        icon.sprite = sprite;
    }

    public void ShowCard()
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

    public void ScaleAnimation()
    {
        AudioManager.instance.PlayShuffleSound();
        transform.DOScale(1f, 0.1f);
        transform.DOLocalRotate(Vector3.zero, 0.1f);
    }
}
