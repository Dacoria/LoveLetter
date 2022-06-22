
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{

    public bool GameEnded;
  
    private List<PlayerScript> CheckWinners()
    {
        var highestScore = -1;
        List<PlayerScript> playersWithHighestScore = new List<PlayerScript>();

        foreach (PlayerScript player in AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted))
        {
            var cardPlayer = player.CurrentCard1();
            var pointsOfCard = DeckSettings.GetCharacterSettings(cardPlayer.Character.Type).Points;

            if (pointsOfCard > highestScore)
            {
                highestScore = pointsOfCard;
                playersWithHighestScore = new List<PlayerScript> { player };
            }
            else if (pointsOfCard == highestScore)
            {
                playersWithHighestScore.Add(player);
            }
        }

        GameText.text = "Game Ended - " + string.Join(", ", playersWithHighestScore.Select(x => x.PlayerName).ToList()) + " Wins!";

        var playersLeft = AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted).ToList();
        if (playersLeft.Count() == 1 && PlayersWhoDiscardedSpies.Any(x => x == playersLeft[0]))
        {
            GameText.text = GameText.text + " + Spy bonus";
        }

        return playersWithHighestScore;
    }

    private bool EndOfGame()
    {
        if (AllPlayers.Count(x => x.PlayerStatus != PlayerStatus.Intercepted) <= 1)
        {
            return true;
        }

        if (Deck.instance.Cards.Count(x => x.Status == CardStatus.InDeck) == 0)
        {
            return true;
        }

        return false;
    }
}