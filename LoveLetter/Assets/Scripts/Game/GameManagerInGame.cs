using System.Linq;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    private int CurrentPlayerIndex;
    private PlayerScript CurrentPlayer() => AllPlayers[CurrentPlayerIndex];         

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
        Deck.instance.SyncDeck();

        NetworkActionEvents.instance.EndCharacterEffect(player, cardId.GetCard().Character.Type, cardId);

        if (!EndOfGame())
        {
            NextPlayer();
        }
        else
        {
            GameEnded = true;
            var winners = CheckWinners();
            NetworkActionEvents.instance.GameEnded(winners);
            
        }
    }  

    private void RemoveCard(int cardId, PlayerScript player)
    {
        var card = cardId.GetCard();
        card.Status = CardStatus.InDiscard;
        
        var remainingCard = Deck.instance.Cards.FirstOrDefault(x => x.PlayerId.GetPlayer() == player);
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
        DealCardToPlayer(CurrentPlayer());

        Deck.instance.SyncDeck();
        NetworkActionEvents.instance.NewPlayerTurn(CurrentPlayer());
    }

    private void DoCardEffect(int cardId, PlayerScript player)
    {
        var card = cardId.GetCard();
        var charSettings = DeckSettings.GetCharacterSettings(card.Character.Type);

        Debug.Log(player.PlayerName + " - DoCardEffect -> " + card.Character.Type);
        charSettings.CharacterEffect.DoEffect(player, cardId);
        NetworkActionEvents.instance.StartCharacterEffect(player, card.Character.Type, cardId);
    }

    private void DealCardToPlayer(PlayerScript player)
    {
        Deck.instance.PlayerDrawsCardFromPile(player);
        Text.GameSync("Turn: " + player.PlayerName);
    }
}