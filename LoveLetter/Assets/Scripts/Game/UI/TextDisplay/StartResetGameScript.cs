using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Pun;

public class StartResetGameScript : MonoBehaviour
{
    public TMP_Text text;
    private bool isGameActive;

    private void Start()
    {
        ActionEvents.NewGameStarted += OnNewGameStarted;
        ActionEvents.GameEnded += OnGameEnded;
    }

    public void OnButtonClick()
    {
        if (isGameActive)
        {
            if(!PhotonNetwork.OfflineMode)
            {
                Textt.GameSync(PhotonNetwork.NickName + " has stopped the game. Waiting to start new game.");
            }
            else
            {
                Textt.GameSync("Game has ended. Waiting to start new game.");
            }
            Textt.ActionSync("");
            GameManager.instance.StopGame();
        }
            
        else
        {
            GameManager.instance.StartGame();
        }
    }

    private void OnGameEnded(List<int> obj)
    {
        isGameActive = false;
        text.text = "Start game";
    }

    private void OnNewGameStarted(List<int> arg1, int arg2)
    {
        text.text = "Stop game";
        isGameActive = true;
    }

    private void OnDestroy()
    {
        ActionEvents.NewGameStarted -= OnNewGameStarted;
        ActionEvents.GameEnded -= OnGameEnded;
    }
}
