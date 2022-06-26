using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckAnimations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ActionEvents.CardDiscarded += OnCardDiscarded;
        ActionEvents.CardsSwitched += OnCardsSwitched;
        ActionEvents.CardsToDeck += OnCardsToDeck;
        ActionEvents.StartCharacterEffect += OnStartCharacterEffect;
        ActionEvents.EndCharacterEffect += OnEndCharacterEffect;
    }

    private void OnStartCharacterEffect(int playerId, CharacterType characterType, int cardId)
    {        //throw new NotImplementedException();
    }

    private void OnEndCharacterEffect(int playerId, CharacterType characterType, int cardId)
    {
        //throw new NotImplementedException();
    }

    private IEnumerator DiscardInXSeconds(int cardId, float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void OnCardsToDeck(List<int> cardIds)
    {
        //throw new NotImplementedException();
    }

    private void OnCardsSwitched(int cardId1, int cardId2)
    {
        //throw new NotImplementedException();
    }

    private void OnCardDiscarded(int cardId)
    {
        //throw new NotImplementedException();
    }

    private void OnDestroy()
    {
        ActionEvents.CardDiscarded -= OnCardDiscarded;
        ActionEvents.CardsSwitched -= OnCardsSwitched;
        ActionEvents.CardsToDeck -= OnCardsToDeck;
        ActionEvents.EndCharacterEffect -= OnEndCharacterEffect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
