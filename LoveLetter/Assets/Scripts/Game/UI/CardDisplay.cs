using UnityEngine;

public class CardDisplay : MonoBehaviour
{    
    public Card Card;
    [ComponentInject] private PlayerScript playerScript;

    private void Awake()
    {
        this.ComponentInject();
    }

    void OnMouseDown()
    {
        PlayCard();
    }

    public void PlayCard()
    {
        GameManager.instance.PlayCard(Card.Id, playerScript);
    }
}
