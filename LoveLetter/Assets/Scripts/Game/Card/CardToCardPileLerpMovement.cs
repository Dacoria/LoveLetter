using UnityEngine;
using System.Linq;
using System;
using System.Collections;

public class CardToCardPileLerpMovement : MonoBehaviour
{
    private Vector2 endPosition;
    private Vector2 startPosition;
    private float desiredDuration = 1.5f;
    private float elapsedTime;
    private Vector2 startScale;
    private Vector2 localScaleTarget;
    private int cardId;


    private bool active;

    private float scaleCorrectionForDiscardPile = 1.13f;

    [SerializeField] private AnimationCurve curve;
    [ComponentInject] public SpriteRenderer SpriteRenderer;

    private UpdateCardDisplayMonoBehaviourAbstract deckPileScript;

    private void Awake()
    {
        this.ComponentInject();
    }

    public void Init(int cardId, Vector2 startPosition, float waitTimeToStartInSeconds = 0)
    {
        var cardStatus = cardId.GetCard().Status;
        if(cardStatus == CardStatus.InDiscard)
        {
            deckPileScript = FindObjectOfType<DiscardPileScript>();
        }
        else if(cardStatus == CardStatus.InDeck)
        {
            deckPileScript = FindObjectOfType<DrawPileScript>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        this.cardId = cardId;
        this.startPosition = startPosition;
        this.startScale = transform.localScale;

        this.endPosition = deckPileScript.GetLocationVisibleCardOnTop().position;
        this.localScaleTarget = new Vector2(scaleCorrectionForDiscardPile, scaleCorrectionForDiscardPile);

        deckPileScript.CardIdsBeingMoved.Add(cardId);
        deckPileScript.UpdateCardDisplay();

        StartCoroutine(ActivateMovement(waitTimeToStartInSeconds));
    }

    private IEnumerator ActivateMovement(float waitTime)
    {
        transform.position = startPosition; 
        yield return new WaitForSeconds(waitTime);
        cardId.GetCard().Status = cardId.GetCard().Status; // opnieuw setten voor datum 
        active = true;
    }

    void Update()
    {
        if(!active)
        {
            return;
        }

        if (elapsedTime > desiredDuration)
        {
            deckPileScript.CardIdsBeingMoved.Remove(cardId);
            deckPileScript.UpdateCardDisplay();
            Destroy(gameObject);
            return;
        }

        elapsedTime += Time.deltaTime;
        float percComplete = elapsedTime / desiredDuration;

        transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percComplete));
        transform.localScale = Vector2.Lerp(startScale, localScaleTarget, percComplete);
    }
}