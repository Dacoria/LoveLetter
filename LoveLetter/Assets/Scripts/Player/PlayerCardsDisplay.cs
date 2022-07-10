using Photon.Pun;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsDisplay : UpdateCardDisplayMonoBehaviourAbstract
{
    [ComponentInject] private PlayerScript playerScript;
    [ComponentInject] private PhotonView photonView;

    public PlayerInteractionCardDisplay Card1Display;
    public PlayerInteractionCardDisplay Card2Display;
    public CardToCardPileLerpMovement EmptyCardPrefab;

    private SpriteRenderer Card1Sprite;
    private SpriteRenderer Card2Sprite;

    private DrawPileScript drawPileScript;

    private Vector2 currentCard1EndPos;
    private Vector2 currentCard2EndPos;

    private Vector2 initCard1EndPos;
    private Vector2 initCard2EndPos;

    private Vector2 avgCardEndPos => (initCard1EndPos + initCard2EndPos) / 2;

    void Awake()
    {
        this.ComponentInject();
        drawPileScript = FindObjectOfType<DrawPileScript>();
        Card1Sprite = Card1Display.GetComponent<SpriteRenderer>();
        Card2Sprite = Card2Display.GetComponent<SpriteRenderer>();               

        cardIdsAlwaysShown = new List<int>();
    }

    private void Start()
    {
        UpdateCardDisplay();
        ActionEvents.RoundEnded += OnRoundEnded;
        ActionEvents.NewRoundStarted += OnNewRoundStarted;
        ActionEvents.StartCharacterEffect += OnStartCharacterEffect;
        ActionEvents.StartShowCardEffect += OnStartShowCardEffect;
        ActionEvents.EndShowCardEffect += OnEndShowCardEffect;
        ActionEvents.DeckCardDrawn += OnDeckCardDrawn;

        initCard1EndPos = Card1Display.transform.position;
        initCard2EndPos = Card2Display.transform.position;

        currentCard1EndPos = initCard1EndPos;
        currentCard2EndPos = initCard2EndPos;
    }

    private void OnStartShowCardEffect(int pId, CharacterType cType, int cardId, int targetCardId)
    {
        cardIdsAlwaysShown.Add(targetCardId);
        UpdateCardDisplay();
    }

    private void OnEndShowCardEffect(int pId, CharacterType cType, int cardId, int targetCardId)
    {
        cardIdsAlwaysShown.Remove(targetCardId);
        UpdateCardDisplay();
    }
    

    private void OnNewRoundStarted(List<int> arg1, int arg2)
    {
        gameEnded = false;
        cardIdsAlwaysShown = new List<int>();
        UpdateCardDisplay();
    }

    private void OnDeckCardDrawn(int obj)
    {
        UpdateCardDisplay();
    }

    private List<int> cardIdsAlwaysShown;

    private void OnStartCharacterEffect(int pId, CharacterType type, int cardId)
    {
        cardIdsAlwaysShown.Add(cardId);
        UpdateCardDisplay();
    }

    private bool gameEnded;

    private void OnRoundEnded(RoundEnded roundEnded)
    {
        gameEnded = true;
        UpdateCardDisplay();
    }

    public Vector2 GetCardPosition(int number) => number == 2 ? currentCard2EndPos : currentCard1EndPos;

    private void OnDestroy()
    {
        ActionEvents.RoundEnded -= OnRoundEnded;
        ActionEvents.NewRoundStarted -= OnNewRoundStarted;
        ActionEvents.StartCharacterEffect -= OnStartCharacterEffect;
        ActionEvents.StartShowCardEffect -= OnStartShowCardEffect;
        ActionEvents.EndShowCardEffect -= OnEndShowCardEffect;
        ActionEvents.DeckCardDrawn -= OnDeckCardDrawn;
    }    

    public override void UpdateCardDisplay()
    {
        if (Deck.instance.Cards != null)
        {
            var card1 = playerScript.CurrentCard1();
            var card2 = playerScript.CurrentCard2();


            if (card1 != null && card2 != null)
            {
                currentCard1EndPos = initCard1EndPos;
                currentCard2EndPos = initCard2EndPos;
            }else
            {
                currentCard1EndPos = avgCardEndPos;
                currentCard2EndPos = avgCardEndPos;
            }

            if (playerScript.PlayerStatus != PlayerStatus.Intercepted &&
               (card1 != null && !Card1Display.gameObject.activeSelf) ||
               (card1 != null && card1?.Id != Card1Display?.Card?.Id))
            {
                StartCardAnimation(card1, Card1Display, currentCard1EndPos);
            }

            if (playerScript.PlayerStatus != PlayerStatus.Intercepted &&
               (card2 != null && !Card2Display.gameObject.activeSelf) ||
               (card2 != null && card2?.Id != Card2Display?.Card?.Id))
            {
                StartCardAnimation(card2, Card2Display, currentCard2EndPos);
            }


            if ((card1 == null && Card1Display.gameObject.activeSelf) ||
               (Card1Display?.Card != null && card1?.Id != Card1Display?.Card?.Id))
            {
                InitCardToDiscardPile(Card1Display, Card1Sprite);
            }

            if ((card2 == null && Card2Display.gameObject.activeSelf) ||
               (Card2Display?.Card != null && card2?.Id != Card2Display?.Card?.Id))
            {
                InitCardToDiscardPile(Card2Display, Card2Sprite);
            }


            Card1Display.Card = card1;
            Card2Display.Card = card2;

            Card1Display.gameObject.SetActive(card1 != null);
            Card2Display.gameObject.SetActive(card2 != null);

            var shouldShowCardToMe = PhotonNetwork.OfflineMode || (photonView.IsMine && !playerScript.IsAi);

            if (card1 != null)
            {
                Card1Sprite.sprite = (gameEnded || shouldShowCardToMe || cardIdsAlwaysShown.Any(x => x == card1.Id)) ? MonoHelper.Instance.GetCharacterSprite(card1.Character.Type) : MonoHelper.Instance.BackgroundCardSprite;
            }
            if (card2 != null)
            {
                Card2Sprite.sprite = (gameEnded || shouldShowCardToMe || cardIdsAlwaysShown.Any(x => x == card2.Id)) ? MonoHelper.Instance.GetCharacterSprite(card2.Character.Type) : MonoHelper.Instance.BackgroundCardSprite;
            }

            if(card1 != null  && (Vector2)Card1Display.transform.position != currentCard1EndPos && !Card1Display.LerpMovement.IsActive)
            {
                Card1Display.LerpMovement.StartMovement(Card1Display.transform.position, currentCard1EndPos);
            }
            if (card2 != null && (Vector2)Card2Display.transform.position != currentCard2EndPos && !Card2Display.LerpMovement.IsActive)
            {
                Card2Display.LerpMovement.StartMovement(Card2Display.transform.position, currentCard2EndPos);
            }


        }
        else
        {
            Card1Display.gameObject.SetActive(false);
            Card2Display.gameObject.SetActive(false);
        }
    }

    private void StartCardAnimation(Card card, PlayerInteractionCardDisplay cardDisplay, Vector2 endpos)
    {
        if(card.PreviousPlayerId > 0 && card.Status == CardStatus.InPlayerHand)
        {
            var prevPlayer = card.PreviousPlayerId.GetPlayer();
            var prevPlayerShowPlayerCards = prevPlayer.GetComponent<PlayerCardsDisplay>();
            var startPosition = prevPlayerShowPlayerCards.GetCardPosition(card.PreviousIndexOfCardInHand);

            cardDisplay.LerpMovement.StartMovement(startPosition, endpos);
        }
        else
        {
            var locCardOnTop = drawPileScript.GetLocationVisibleCardOnTop();
            cardDisplay.LerpMovement.StartMovement(locCardOnTop.position, endpos);
        }
    }

    public override Transform GetLocationVisibleCardOnTop()
    {
        return transform;
    }

    private void InitCardToDiscardPile(PlayerInteractionCardDisplay origCardDisplayToCopy, SpriteRenderer cardSpriteToCopy)
    {
        var cardDisplay = Instantiate(EmptyCardPrefab, transform);
        cardDisplay.SpriteRenderer.sprite = cardSpriteToCopy.sprite;

        var cardOnDisplay = origCardDisplayToCopy.Card.Id.GetCard();
        var isPlayedPrince = cardOnDisplay.CardIsPlayed && cardOnDisplay.Character.Type == CharacterType.Prince;
        var isPlayedKing = cardOnDisplay.CardIsPlayed && cardOnDisplay.Character.Type == CharacterType.King;
        var isPlayedChancellor = cardOnDisplay.CardIsPlayed && cardOnDisplay.Character.Type == CharacterType.Chancellor;

        if (cardOnDisplay.Status == CardStatus.InDiscard)
        {
            cardDisplay.SpriteRenderer.sprite = MonoHelper.Instance.GetCharacterSprite(cardOnDisplay.Character.Type);
        }

        var waitTimeToDiscard = 0.0f;
        if(isPlayedPrince)
        {
            waitTimeToDiscard = 0.8f;
        }
        else if (isPlayedKing)
        {
            waitTimeToDiscard = 0.8f;
        }
        else if (isPlayedChancellor)
        {
            waitTimeToDiscard = 0.8f;
        }
        cardDisplay.Init(origCardDisplayToCopy.Card.Id, origCardDisplayToCopy.transform.position, waitTimeToStartInSeconds: waitTimeToDiscard);
    }
}