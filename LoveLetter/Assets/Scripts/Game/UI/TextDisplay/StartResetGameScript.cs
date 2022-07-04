using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class StartResetGameScript : MonoBehaviour
{
    public TMP_Text text;
    public Button StartRound;

    private bool isRoundActive;

    private void Start()
    {
        ActionEvents.NewRoundStarted += OnNewRoundStarted;
        ActionEvents.RoundEnded += OnRoundEnded;
        ActionEvents.GameEnded += OnGameEnded;
    }    

    public void OnButtonClick()
    {
        if (isRoundActive)
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
            GameManager.instance.StopRound();
        }
            
        else
        {
            GameManager.instance.StartRound();
        }
    }

    private void OnRoundEnded(RoundEnded roundEnded)
    {
        isRoundActive = false;
        text.text = "Start Round";        
    }

    private void OnGameEnded()
    {
        Destroy(gameObject);
    }

    private void OnNewRoundStarted(List<int> arg1, int arg2)
    {
        text.text = "Stop Round";
        isRoundActive = true;
    }

    private void OnDestroy()
    {
        ActionEvents.NewRoundStarted -= OnNewRoundStarted;
        ActionEvents.RoundEnded -= OnRoundEnded;
        ActionEvents.GameEnded -= OnGameEnded;
    }
}
