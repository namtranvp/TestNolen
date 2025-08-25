using UnityEngine;
using UnityEngine.EventSystems;

public class CardItem : MonoBehaviour, IPointerClickHandler
{
    private int id;
    private GameManager gameManager;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card clicked: " + gameObject.name);
    }

    public void SetId(int newId, GameManager manager)
    {
        id = newId;
        gameManager = manager;
        gameObject.name = "Card_" + id;
    }
}
