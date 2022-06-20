using System.Linq;
using UnityEngine;

public class ShowPlayerCards : MonoBehaviour
{
    [ComponentInject] private PlayerScript playerScript;
    public CardDisplay Card1Display;
    public CardDisplay Card2Display;
    public CardDisplay Card3Display;
    public CardDisplay Card4Display;

    private SpriteRenderer Card1Sprite;
    private SpriteRenderer Card2Sprite;
    private SpriteRenderer Card3Sprite;
    private SpriteRenderer Card4Sprite;

    void Awake()
    {
        this.ComponentInject();
        Card1Sprite = Card1Display.GetComponent<SpriteRenderer>();
        Card2Sprite = Card2Display.GetComponent<SpriteRenderer>();
        Card3Sprite = Card3Display.GetComponent<SpriteRenderer>();
        Card4Sprite = Card4Display.GetComponent<SpriteRenderer>();
    }

    private int counter;

    void Update()
    {
        if(counter == 0)
        {
            UpdateCardDisplay();
        }

        counter++;
        if(counter > 20)
        {
            counter = 0;
        }
    }

    private void UpdateCardDisplay()
    {
        if (DeckManager.instance.Deck != null)
        {
            var card1 = playerScript.CurrentCard1();
            var card2 = playerScript.CurrentCard2();
            var card3 = playerScript.CurrentCard3();
            var card4 = playerScript.CurrentCard4();

            Card1Display.Card = card1;
            Card2Display.Card = card2;
            Card3Display.Card = card3;
            Card4Display.Card = card4;

            Card1Display.gameObject.SetActive(card1 != null);
            Card2Display.gameObject.SetActive(card2 != null);
            Card3Display.gameObject.SetActive(card3 != null);
            Card4Display.gameObject.SetActive(card4 != null);


            if (card1 != null)
            {
                Card1Sprite.sprite = card1.Character.Sprite;
            }
            if (card2 != null)
            {
                Card2Sprite.sprite = card2.Character.Sprite;
            }
            if (card3 != null)
            {
                Card3Sprite.sprite = card3.Character.Sprite;
            }
            if (card4 != null)
            {
                Card4Sprite.sprite = card4.Character.Sprite;
            }
        }
        else
        {
            Card1Display.gameObject.SetActive(false);
            Card2Display.gameObject.SetActive(false);
            Card3Display.gameObject.SetActive(false);
            Card4Display.gameObject.SetActive(false);
        }
    }
}
