using Photon.Pun;
using System;
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

    private void OnNewGameStarted()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ActionEvents.NewGameStarted -= OnNewGameStarted;
    }
}
