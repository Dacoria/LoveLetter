using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class NetworkActionEvents: MonoBehaviour
{
    [ComponentInject] private PhotonView photonView;
    public static NetworkActionEvents instance;

    private void Awake()
    {
        instance = this;
        this.ComponentInject();
    }

    public void NewGameStarted()
    {
        photonView.RPC("RPC_AE_NewGameStarted", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_AE_NewGameStarted()
    {
        ActionEvents.NewGameStarted?.Invoke();
    }

    public void NewPlayerTurn(PlayerScript ps)
    {
        photonView.RPC("RPC_AE_NewPlayerTurn", RpcTarget.All, ps);
    }

    [PunRPC]
    public void RPC_AE_NewPlayerTurn(PlayerScript ps)
    {
        ActionEvents.NewPlayerTurn?.Invoke(ps);
    }

    public void StartCharacterEffect(PlayerScript ps, CharacterType ct, int cId)
    {
        photonView.RPC("RPC_AE_StartCharacterEffect", RpcTarget.All, ps, ct, cId);
    }

    [PunRPC]
    public void RPC_AE_StartCharacterEffect(PlayerScript ps, CharacterType ct, int cId)
    {
        ActionEvents.StartCharacterEffect?.Invoke(ps, ct, cId);
    }

    public void EndCharacterEffect(PlayerScript ps, CharacterType ct, int cId)
    {
        photonView.RPC("RPC_AE_EndCharacterEffect", RpcTarget.All, ps, ct, cId);
    }

    [PunRPC]
    public void RPC_AE_EndCharacterEffect(PlayerScript ps, CharacterType ct, int cId)
    {
        ActionEvents.EndCharacterEffect?.Invoke(ps, ct, cId);
    }

    public void PlayerStatusChange(PlayerScript ps, PlayerStatus pStatus)
    {
        photonView.RPC("RPC_AE_PlayerStatusChange", RpcTarget.All, ps, pStatus);
    }

    [PunRPC]
    public void RPC_AE_PlayerStatusChange(PlayerScript ps, PlayerStatus pStatus)
    {
        ActionEvents.PlayerStatusChange?.Invoke(ps, pStatus);
    }

    public void GameEnded(List<PlayerScript> pWon)
    {
        photonView.RPC("RPC_AE_GameEnded", RpcTarget.All, pWon);
    }

    [PunRPC]
    public void RPC_AE_GameEnded(List<PlayerScript> pWon)
    {
        ActionEvents.GameEnded?.Invoke(pWon);
    }
}