using Photon.Pun;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlayerCards : UpdateCardDisplayMonoBehaviourAbstract
{
    [ComponentInject] private PlayerScript playerScript;
    [ComponentInject] private PhotonView photonView;

    public PlayerCardDisplay Card1Display;
    public PlayerCardDisplay Card2Display;
    public CardToCardPileLerpMovement EmptyCardPrefab;

    private SpriteRenderer Card1Sprite;
    private SpriteRenderer Card2Sprite;

    private DrawPileScript drawPileScript;

    private Vector2 card1EndPos;
    private Vector2 card2EndPos;

    void Awake()
    {
        this.ComponentInject();
        drawPileScript = FindObjectOfType<DrawPileScript>();
        Card1Sprite = Card1Display.GetComponent<SpriteRenderer>();
        Card2Sprite = Card2Display.GetComponent<SpriteRenderer>();

        card1EndPos = Card1Display.transform.position;
        card2EndPos = Card2Display.transform.position;

        cardIdsAlwaysShown = new List<int>();
    }

    private void Start()
    {
        UpdateCardDisplay();
        ActionEvents.GameEnded += OnGameEnded;
        ActionEvents.NewGameStarted += OnNewGameStarted;
        ActionEvents.StartCharacterEffect += OnStartCharacterEffect;
    }

    private void OnNewGameStarted(List<int> arg1, int arg2)
    {
        gameEnded = false;
        cardIdsAlwaysShown = new List<int>();
        UpdateCardDisplay();
    }

    private bool gameEnded;

    private void OnGameEnded(List<int> obj)
    {
        gameEnded = true;
        UpdateCardDisplay();
    }

    private void OnDestroy()
    {
        ActionEvents.GameEnded -= OnGameEnded;
        ActionEvents.NewGameStarted -= OnNewGameStarted;
        ActionEvents.StartCharacterEffect -= OnStartCharacterEffect;
    }

    private List<int> cardIdsAlwaysShown;

    private void OnStartCharacterEffect(int pId, CharacterType type, int cardId)
    {
        cardIdsAlwaysShown.Add(cardId);
        UpdateCardDisplay();
    }

    public override void UpdateCardDisplay()
    {
        if (Deck.instance.Cards != null)
        {
            var card1 = playerScript.CurrentCard1();
            var card2 = playerScript.CurrentCard2();

            //playerScript.PlayerStatus == PlayerStatus.Intercepted

            if((card1 != null && !Card1Display.gameObject.activeSelf) ||
               (card1 != null && card1?.Id != Card1Display?.Card?.Id))
            {                
                var locCardOnTop = drawPileScript.GetLocationVisibleCardOnTop();
                Card1Display.LerpMovement.StartMovement(locCardOnTop.position, card1EndPos);                
            }

            if ((card2 != null && !Card2Display.gameObject.activeSelf) ||
               (card2 != null && card2?.Id != Card2Display?.Card?.Id))
            {
                var locCardOnTop = drawPileScript.GetLocationVisibleCardOnTop();
                Card2Display.LerpMovement.StartMovement(locCardOnTop.position, card2EndPos);
            }


            if ((card1 == null && Card1Display.gameObject.activeSelf) ||
               (Card1Display?.Card != null && card1?.Id != Card1Display?.Card?.Id))
            {
                var cardDisplay = Instantiate(EmptyCardPrefab, transform);
                cardDisplay.SpriteRenderer.sprite = Card1Sprite.sprite;
                cardDisplay.Init(Card1Display.Card.Id, Card1Display.transform.position);
            }

            if ((card2 == null && Card2Display.gameObject.activeSelf) ||
               (Card2Display?.Card != null && card2?.Id != Card2Display?.Card?.Id))
            {
                var cardDisplay = Instantiate(EmptyCardPrefab, transform);
                cardDisplay.SpriteRenderer.sprite = Card2Sprite.sprite;
                cardDisplay.Init(Card2Display.Card.Id, Card2Display.transform.position);
            }


            Card1Display.Card = card1;
            Card2Display.Card = card2;

            Card1Display.gameObject.SetActive(card1 != null);
            Card2Display.gameObject.SetActive(card2 != null);


            if (card1 != null)
            {
                Card1Sprite.sprite = (gameEnded || photonView.IsMine || cardIdsAlwaysShown.Any(x => x == card1.Id)) ? MonoHelper.Instance.GetCharacterSprite(card1.Character.Type) : MonoHelper.Instance.BackgroundCardSprite;
            }
            if (card2 != null)
            {
                Card2Sprite.sprite = (gameEnded || photonView.IsMine || cardIdsAlwaysShown.Any(x => x == card2.Id)) ? MonoHelper.Instance.GetCharacterSprite(card2.Character.Type) : MonoHelper.Instance.BackgroundCardSprite;
            }           
        }
        else
        {
            Card1Display.gameObject.SetActive(false);
            Card2Display.gameObject.SetActive(false);
        }
    }

    public override Transform GetLocationVisibleCardOnTop()
    {
        return transform;
    }
}