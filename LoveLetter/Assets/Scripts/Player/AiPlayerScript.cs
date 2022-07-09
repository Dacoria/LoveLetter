using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections;

public class AiPlayerScript : MonoBehaviour
{
    [ComponentInject] private PlayerScript PlayerScript;

    private void Awake()
    {
        this.ComponentInject();
    }

    private void Start()
    {
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
    }

    private IEnumerator PlayCard()
    {
        yield return new WaitForSeconds(2f);
        var options = Deck.instance.Cards.Where(x => x?.PlayerId == PlayerScript.PlayerId).ToList();
        options.Shuffle();

        foreach(var card in options)
        {
            if(card.Character.Type == CharacterType.Princess)
            {
                continue;
            }

            var charSettings = DeckSettings.GetCharacterSettings(card.Character.Type);
            var canDoEffect = charSettings.CharacterEffect.CanDoEffect(PlayerScript, card.Id);
            if(canDoEffect)
            {
                GameManager.instance.PlayCard(card.Id, PlayerScript.PlayerId);
                break;
            }
        }
    }

    public void DoCardChoice(Action<string> callback, List<string> options, CharacterType characterType, int currentCardId)
    {
        StartCoroutine(DoCardChoiceAfterXSeconds(callback, options, characterType, currentCardId));
    }

    public IEnumerator DoCardChoiceAfterXSeconds(Action<string> callback, List<string> options, CharacterType characterType, int currentCardId)
    {
        yield return new WaitForSeconds(4f);
        options.Shuffle();
        callback(options.First());
    }

    private void OnNewPlayerTurn(int pId)
    {
        if (pId == PlayerScript.PlayerId)
        {
            StartCoroutine(DrawCard());
        }
    }

    private IEnumerator DrawCard()
    {
        yield return new WaitForSeconds(0.5f);
        Deck.instance.PlayerDrawsCardFromPileSync(PlayerScript.PlayerId);

        StartCoroutine(PlayCard());
    }

    private void OnDestroy()
    {
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
    }
}