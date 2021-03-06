using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameTexts : MonoBehaviour
{
    public TMP_Text GameText;
    public TMP_Text ActionText;

    private void Start()
    {
        ActionEvents.NewRoundStarted += OnNewRoundStarted;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.DeckCardDrawn += OnDeckCardDrawn;
    }

    private void OnDestroy()
    {
        ActionEvents.NewRoundStarted -= OnNewRoundStarted;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.DeckCardDrawn -= OnDeckCardDrawn;
    }

    private void OnNewPlayerTurn(int playerId)
    {
        UpdateTurnCurrentPlayerGameText(playerId);
    }

    private void OnNewRoundStarted(List<int> listPlayers, int playerId)
    {
        UpdateTurnCurrentPlayerGameText(playerId);
    }

    private void UpdateTurnCurrentPlayerGameText(int playerId)
    {
        var myPlayer = NetworkHelper.Instance.GetMyPlayerScript();
        if (myPlayer.PlayerId == playerId)
        {
            if(Deck.instance.Cards.Count > 0)
            {
                GameText.text = "Your turn, pick a card!";
            }
            else
            {
                GameText.text = "Your turn";
            }
        }
        else
        {
            GameText.text = "Turn: " + NetworkHelper.Instance.GetPlayerScriptById(playerId).PlayerName;
        }
    }

    private void OnDeckCardDrawn(int playerId)
    {
        if (NetworkHelper.Instance.GetMyPlayerScript().PlayerId == playerId)
        {
            GameText.text = "Your turn";
        }
    }
}

