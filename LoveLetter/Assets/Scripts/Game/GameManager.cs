
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int CurrentPlayerIndex;
    private List<PlayerScript> AllPlayers;
    private PlayerScript CurrentPlayer() => AllPlayers[CurrentPlayerIndex];

    public TMP_Text GameText;
    public List<PlayerScript> PlayersWhoDiscardedSpies;


    public bool GameEnded;

    private void Awake()
    {
        instance = this;
        GameText.text = "Waiting for game to start";
    }

    public void StartGame()
    {
        // bepaling wanneer alle spelers compleet zijn;
        AllPlayers = NetworkHelper.Instance.GetPlayers();

        CurrentPlayerIndex = 0;
        GameEnded = false;
        PlayersWhoDiscardedSpies = new List<PlayerScript>();
        MonoHelper.Instance.SetActionText("");

        DeckManager.instance.CreateDeck();
        StartNewGameForPlayers();

        GiveCardToCurrentPlayer();
    }

    private void GiveCardToCurrentPlayer()
    {
        DealCardToPlayer(CurrentPlayer());
    }

    private bool EffectBeingPlayed;

    public void PlayCard(int cardId, PlayerScript player)
    {
        if(!GameEnded && player == CurrentPlayer() && !EffectBeingPlayed)
        {
            EffectBeingPlayed = true;
            DoCardEffect(cardId, player);         
        }
        else if (EffectBeingPlayed)
        {
            Debug.Log(player.PlayerName + " wants to do a move, but is already doing another move");
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
        EffectBeingPlayed = false;
        RemoveCard(cardId, player);
        if (!EndOfGame())
        {
            NextPlayer();
        }
        else
        {
            GameEnded = true;
            CheckWinner();
        }
    }

    private void CheckWinner()
    {
        var highestScore = -1;
        List<PlayerScript> playersWithHighestScore = new List<PlayerScript>();

        foreach (PlayerScript player in AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted))
        {
            var cardPlayer = player.CurrentCard1();
            var pointsOfCard = DeckSettings.GetCharacterSettings(cardPlayer.Character.CharacterType).Points;

            if(pointsOfCard > highestScore)
            {
                highestScore = pointsOfCard;
                playersWithHighestScore = new List<PlayerScript> { player };
            }
            else if(pointsOfCard == highestScore)
            {
                playersWithHighestScore.Add(player);
            }
        }

        GameText.text = "Game Ended - " + string.Join(", ", playersWithHighestScore.Select(x => x.PlayerName).ToList()) + " Wins!";

        var playersLeft = AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted).ToList();
        if (playersLeft.Count() == 1 && PlayersWhoDiscardedSpies.Any(x => x == playersLeft[0]))
        {
            GameText.text = GameText.text + " " + playersLeft[0].PlayerName + " gets a Spy bonus point!";
        }
    }

    private bool EndOfGame()
    {
        if(AllPlayers.Count(x => x.PlayerStatus != PlayerStatus.Intercepted) <= 1)
        {
            return true;
        }

        if(DeckManager.instance.Deck.Cards.Count(x => x.Status == CardStatus.InDeck) == 0)
        {
            return true;
        }

        return false;
    }

    private void RemoveCard(int cardId, PlayerScript player)
    {
        var card = DeckManager.instance.Deck.Cards.Single(x => x.Player == player && x.Id == cardId);
        card.Status = CardStatus.InDiscard;
        
        var remainingCard = DeckManager.instance.Deck.Cards.FirstOrDefault(x => x.Player == player);
        if(remainingCard != null)
        {
            remainingCard.IndexOfCardInHand = 1;
        }
    }

    private void NextPlayer()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % AllPlayers.Count;
        CurrentPlayer().PlayerStatus = PlayerStatus.Normal;
        if (DeckManager.instance.Deck.Cards.Count(x => x.Status == CardStatus.InDeck)  > 0)
        {
            GiveCardToCurrentPlayer();
        }
    }

    private void DoCardEffect(int cardId, PlayerScript player)
    {
        var card = DeckManager.instance.Deck.Cards.Single(x => x.Id == cardId);
        var charSettings = DeckSettings.GetCharacterSettings(card.Character.CharacterType);
        charSettings.CharacterEffect.DoEffect(player, cardId);
    }

    private void StartNewGameForPlayers()
    {
        foreach (var player in AllPlayers)
        {
            DealCardToPlayer(player);
        }
    }

    private void DealCardToPlayer(PlayerScript player)
    {
        DeckManager.instance.PlayerDrawsCardFromPile(player);
        //player.DrawCard();
        GameText.text = "Turn: " + player.PlayerName;
    }
}