
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public bool GameEnded;

    public void StopGame()
    {
        GameEnded = true;
        NetworkActionEvents.instance.GameEnded(new List<int>());
    }

    private Dictionary<int, int> CheckWinners()
    {
        var highestScore = -1;
        var playersWithHighestScore = new Dictionary<int, int>();

        foreach (PlayerScript player in AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted))
        {
            var cardPlayer = player.CurrentCard1() ?? player.CurrentCard2();
            var pointsOfCard = DeckSettings.GetCharacterSettings(cardPlayer.Character.Type).Points;

            if (pointsOfCard > highestScore)
            {
                highestScore = pointsOfCard;
                playersWithHighestScore = new Dictionary<int, int>();
                playersWithHighestScore.Add(player.PlayerId, 1);
            }
            else if (pointsOfCard == highestScore)
            {
                playersWithHighestScore.Add(player.PlayerId, 1);
            }
        }


        var playersLeft = AllPlayers.Where(x => x.PlayerStatus != PlayerStatus.Intercepted).ToList();
        var extraSpyText = "";
        if (playersLeft.Count() == 1 && PlayersWhoDiscardedSpies.Any(x => x == playersLeft[0].PlayerId))
        {
            playersWithHighestScore = new Dictionary<int, int>();
            playersWithHighestScore.Add(playersLeft.First().PlayerId, 2);
            extraSpyText = " + Spy bonus";
        }       

        Textt.GameSync("Game Ended - " + string.Join(", ", playersWithHighestScore.Select(x => x.Key.GetPlayer().PlayerName).ToList()) + " Wins!" + extraSpyText);

        return playersWithHighestScore;
    }

    private void OnGameEnded(List<int> obj)
    {
        GameEnded = true;
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