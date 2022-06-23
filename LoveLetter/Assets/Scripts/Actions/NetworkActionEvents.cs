using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NetworkActionEvents: MonoBehaviour
{
    [ComponentInject] private PhotonView photonView;
    public static NetworkActionEvents instance;

    private void Awake()
    {
        instance = this;
        this.ComponentInject();
    }

    public void NewGameStarted(List<int> playerIds, int currentPlayerId)
    {
        photonView.RPC("RPC_AE_NewGameStarted", RpcTarget.All, playerIds.ToArray(), currentPlayerId);
    }

    [PunRPC]
    public void RPC_AE_NewGameStarted(int[] playerIds, int currentPlayerId)
    {
        ActionEvents.NewGameStarted?.Invoke(playerIds.ToList(), currentPlayerId);
    }

    public void NewPlayerTurn(int pId)
    {
        photonView.RPC("RPC_AE_NewPlayerTurn", RpcTarget.All, pId);
    }

    [PunRPC]
    public void RPC_AE_NewPlayerTurn(int pId)
    {
        ActionEvents.NewPlayerTurn?.Invoke(pId);
    }

    public void StartCharacterEffect(int pId, CharacterType ct, int cId)
    {
        photonView.RPC("RPC_AE_StartCharacterEffect", RpcTarget.All, pId, ct, cId);
    }

    [PunRPC]
    public void RPC_AE_StartCharacterEffect(int pId, CharacterType ct, int cId)
    {
        ActionEvents.StartCharacterEffect?.Invoke(pId, ct, cId);
    }

    public void EndCharacterEffect(int pId, CharacterType ct, int cId)
    {
        photonView.RPC("RPC_AE_EndCharacterEffect", RpcTarget.All, pId, ct, cId);
    }

    [PunRPC]
    public void RPC_AE_EndCharacterEffect(int pId, CharacterType ct, int cId)
    {
        ActionEvents.EndCharacterEffect?.Invoke(pId, ct, cId);
    }

    public void PlayerStatusChange(int pId, PlayerStatus pStatus)
    {
        photonView.RPC("RPC_AE_PlayerStatusChange", RpcTarget.All, pId, pStatus);
    }

    [PunRPC]
    public void RPC_AE_PlayerStatusChange(int pId, PlayerStatus pStatus)
    {
        ActionEvents.PlayerStatusChange?.Invoke(pId, pStatus);
    }

    public void GameEnded(List<int> pIdsWon)
    {
        photonView.RPC("RPC_AE_GameEnded", RpcTarget.All, pIdsWon.ToArray());
    }

    [PunRPC]
    public void RPC_AE_GameEnded(int[] pIdsWon)
    {
        ActionEvents.GameEnded?.Invoke(pIdsWon.ToList());
    }
}