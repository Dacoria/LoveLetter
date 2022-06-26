using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDummyPlayerScript : MonoBehaviour
{
    void Start()
    {
        if(!PhotonNetwork.OfflineMode)
        {
            Destroy(gameObject);
        }

        ActionEvents.NewGameStarted += OnNewGameStarted;
    }

    private void OnNewGameStarted(List<int> p, int q)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ActionEvents.NewGameStarted -= OnNewGameStarted;
    }
}