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

    public void NewRoundStarted(List<int> playerIds, int currentPlayerId)
    {
        photonView.RPC("RPC_AE_NewRoundStarted", RpcTarget.All, playerIds.ToArray(), currentPlayerId);
    }

    [PunRPC]
    public void RPC_AE_NewRoundStarted(int[] playerIds, int currentPlayerId)
    {
        ActionEvents.NewRoundStarted?.Invoke(playerIds.ToList(), currentPlayerId);
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

    public void RoundEnded(RoundEnded roundEnded)
    {
        var roundEndedJson = JsonUtility.ToJson(roundEnded);
        photonView.RPC("RPC_AE_RoundEnded", RpcTarget.All, roundEndedJson);
    }

    [PunRPC]
    public void RPC_AE_RoundEnded(string roundEndedJson)
    {
        var roundEnded = JsonUtility.FromJson<RoundEnded>(roundEndedJson);
        ActionEvents.RoundEnded?.Invoke(roundEnded);
    }

    public void CardsToDeck(List<int> cardidsToDeck)
    {
        photonView.RPC("RPC_AE_CardsToDeck", RpcTarget.All, cardidsToDeck.ToArray());
    }

    [PunRPC]
    public void RPC_AE_CardsToDeck(int[] cardIdsToDeck)
    {
        ActionEvents.CardsToDeck?.Invoke(cardIdsToDeck.ToList());
    }

    public void CardDiscarded(int cardid)
    {
        photonView.RPC("RPC_AE_CardDiscarded", RpcTarget.All, cardid);
    }

    [PunRPC]
    public void RPC_AE_CardDiscarded(int cardid)
    {
        ActionEvents.CardDiscarded?.Invoke(cardid);
    }

    public void CardsSwitched(int cardid1, int cardid2)
    {
        photonView.RPC("RPC_AE_CardsSwitched", RpcTarget.All, cardid1, cardid2);
    }

    [PunRPC]
    public void RPC_AE_CardsSwitched(int cardid1, int cardid2)
    {
        ActionEvents.CardsSwitched?.Invoke(cardid1, cardid2);
    }

    public void StartComparingCards (int playerId, int cardId, int otherPlayerId, int otherCardId)
    {
        photonView.RPC("RPC_AE_StartComparingCards", RpcTarget.All, playerId, cardId, otherPlayerId, otherCardId);
    }

    [PunRPC]
    public void RPC_AE_StartComparingCards(int playerId, int cardId, int otherPlayerId, int otherCardId)
    {
        ActionEvents.StartComparingCards?.Invoke(playerId, cardId, otherPlayerId, otherCardId);
    }

    public void FinishedComparingCards(int playerId, int cardId, int otherPlayerId, int otherCardId)
    {
        photonView.RPC("RPC_AE_FinishedComparingCards", RpcTarget.All, playerId, cardId, otherPlayerId, otherCardId);
    }

    [PunRPC]
    public void RPC_AE_FinishedComparingCards(int playerId, int cardId, int otherPlayerId, int otherCardId)
    {
        ActionEvents.FinishedComparingCards?.Invoke(playerId, cardId, otherPlayerId, otherCardId);
    }
}