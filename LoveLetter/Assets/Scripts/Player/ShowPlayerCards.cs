using System.Linq;
using UnityEngine;

public class ShowPlayerCards : MonoBehaviour
{
    [ComponentInject] private PlayerScript playerScript;
    public CardDisplay Card1Display;
    public CardDisplay Card2Display;

    private SpriteRenderer Card1Sprite;
    private SpriteRenderer Card2Sprite;

    void Awake()
    {
        this.ComponentInject();
        Card1Sprite = Card1Display.GetComponent<SpriteRenderer>();
        Card2Sprite = Card2Display.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(DeckManager.instance.Deck != null)
        {
            Card1Display.Card = playerScript.CurrentCard1;
            Card2Display.Card = playerScript.CurrentCard2;

            var currentCardsOfPlayer = DeckManager.instance.Deck.Cards.Where(x => x.Player != null && x.Player == playerScript).ToList();

            Card1Display.gameObject.SetActive(playerScript.CurrentCard1 != null);
            Card2Display.gameObject.SetActive(playerScript.CurrentCard2 != null);


            if (playerScript.CurrentCard1 != null)
            {
                Card1Sprite.sprite = playerScript.CurrentCard1.Character.Sprite;
            }            
            if (playerScript.CurrentCard2 != null)
            {
                Card2Sprite.sprite = playerScript.CurrentCard2.Character.Sprite;
            }

            if(currentCardsOfPlayer.Count() == 1)
            {
                Card1Display.transform.localPosition = new Vector2(0, Card1Display.transform.localPosition.y);
            }
            else
            {
                Card1Display.transform.localPosition = new Vector2(-1.6f, Card1Display.transform.localPosition.y);
                Card2Display.transform.localPosition = new Vector2(1.6f, Card2Display.transform.localPosition.y);
            }
        }
        else
        {
            Card1Display.gameObject.SetActive(false);
            Card2Display.gameObject.SetActive(false);
        }
    }
}
