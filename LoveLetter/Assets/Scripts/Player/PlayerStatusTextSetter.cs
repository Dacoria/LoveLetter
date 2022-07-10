using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatusTextSetter : MonoBehaviour
{
    [ComponentInject] private TMP_Text playerNameText;
    [ComponentInject] private PlayerScript player;
    public SpriteRenderer ShieldSprite;

    private void Awake()
    {
        this.ComponentInject();
    }

    void Start()
    {
        ActionEvents.NewRoundStarted += OnNewGameStarted;
        ActionEvents.RoundEnded += OnRoundEnded;
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
    }
    private void OnNewPlayerTurn(int currentPlayerId)
    {
        playerNameText.fontStyle = player.PlayerId == currentPlayerId ? FontStyles.Underline : FontStyles.Normal;

        if (currentPlayerId == player.PlayerId)
        {
            playerNameText.color = new Color(1, 117f / 255, 0);   
        }
        else
        {
            playerNameText.color =  player.PlayerStatus == PlayerStatus.Normal ? Color.black :
                                    player.PlayerStatus == PlayerStatus.Intercepted ? Color.red :
                                    player.PlayerStatus == PlayerStatus.Protected ? Color.blue :
                                    Color.white;
        }
        
        ShieldSprite.color = new Color(ShieldSprite.color.r, ShieldSprite.color.g, ShieldSprite.color.b, player.PlayerStatus == PlayerStatus.Protected ? 1 : 0);
        
    }

    private void OnNewGameStarted(List<int> a, int currentPlayerId)
    {
        playerNameText.color = Color.white;
        OnNewPlayerTurn(currentPlayerId);
    }

    private void OnRoundEnded(RoundEnded roundEnded)
    {
        playerNameText.fontStyle = FontStyles.Normal;
        if (roundEnded.PlayerScores.Any(x => x.PlayerId == player.PlayerId && x.WonRound))
        {
            playerNameText.color = Color.green;
        }
        else if(playerNameText.color == Color.blue)
        {
            playerNameText.color = Color.black;
        }
    }    

    private void OnDestroy()
    {
        ActionEvents.NewRoundStarted -= OnNewGameStarted;
        ActionEvents.RoundEnded -= OnRoundEnded;
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
    }
}
