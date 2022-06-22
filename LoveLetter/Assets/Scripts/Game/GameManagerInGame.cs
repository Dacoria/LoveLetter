using System.Linq;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    private int CurrentPlayerIndex;
    private PlayerScript CurrentPlayer() => AllPlayers[CurrentPlayerIndex];


    private void GiveCardToCurrentPlayer()
    {
        DealCardToPlayer(CurrentPlayer());
    }

    public void PlayCard(int cardId, PlayerScript player)
    {
        if(!GameEnded && player == CurrentPlayer() && !MonoHelper.Instance.GetModal().IsActive)
        {            
            DoCardEffect(cardId, player);         
        }       
        else if(GameEnded)
        {
            Debug.Log(player.PlayerName + " wants to do a move, but the game has already ended");
        }
        else
        {
            Debug.Log(player.PlayerName + " wants to do a move, but it is not his turn");
        }
    }

    public void CardEffectPlayed(int cardId, PlayerScript player)
    {
        RemoveCard(cardId, player);
        ActionEvents.EndCharacterEffect?.Invoke(player, cardId.GetCard().Character.Type, cardId);

        if (!EndOfGame())
        {
            NextPlayer();
        }
        else
        {
            GameEnded = true;
            var winners = CheckWinners();
            ActionEvents.GameEnded?.Invoke(winners);
            
        }
    }  

    private void RemoveCard(int cardId, PlayerScript player)
    {
        var card = cardId.GetCard();
        card.Status = CardStatus.InDiscard;
        
        var remainingCard = Deck.instance.Cards.FirstOrDefault(x => x.Player == player);
        if(remainingCard != null)
        {
            remainingCard.IndexOfCardInHand = 1;
        }
    }

    private void NextPlayer()
    {
        do
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % AllPlayers.Count;
        } 
        while (CurrentPlayer().PlayerStatus == PlayerStatus.Intercepted);
        
        CurrentPlayer().PlayerStatus = PlayerStatus.Normal;
        GiveCardToCurrentPlayer();
        ActionEvents.NewPlayerTurn?.Invoke();
    }

    private void DoCardEffect(int cardId, PlayerScript player)
    {
        var card = cardId.GetCard();
        var charSettings = DeckSettings.GetCharacterSettings(card.Character.Type);

        Debug.Log(player.PlayerName + " - DoCardEffect -> " + card.Character.Type);
        charSettings.CharacterEffect.DoEffect(player, cardId);
        ActionEvents.StartCharacterEffect?.Invoke(player, card.Character.Type, cardId);
    }

    private void DealCardToPlayer(PlayerScript player)
    {
        Deck.instance.PlayerDrawsCardFromPile(player);
        GameText.text = "Turn: " + player.PlayerName;
    }
}