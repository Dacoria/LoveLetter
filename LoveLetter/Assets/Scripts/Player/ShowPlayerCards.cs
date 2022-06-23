using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerCards : UpdateCardDisplayMonoBehaviourAbstract
{
    [ComponentInject] private PlayerScript playerScript;
    [ComponentInject] private PhotonView photonView;

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

    private void Start()
    {
        UpdateCardDisplay();        
    }

    public override void UpdateCardDisplay()
    {
        if (Deck.instance.Cards != null)
        {
            var card1 = playerScript.CurrentCard1();
            var card2 = playerScript.CurrentCard2();

            Card1Display.Card = card1;
            Card2Display.Card = card2;

            Card1Display.gameObject.SetActive(card1 != null);
            Card2Display.gameObject.SetActive(card2 != null);


            if (card1 != null)
            {
                Card1Sprite.sprite = photonView.IsMine ? MonoHelper.Instance.GetCharacterSprite(card1.Character.Type) : MonoHelper.Instance.BackgroundCardSprite;
            }
            if (card2 != null)
            {
                Card2Sprite.sprite = photonView.IsMine ? MonoHelper.Instance.GetCharacterSprite(card2.Character.Type) : MonoHelper.Instance.BackgroundCardSprite;
            }           
        }
        else
        {
            Card1Display.gameObject.SetActive(false);
            Card2Display.gameObject.SetActive(false);
        }
    }
}