using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkHelper : MonoBehaviourPunCallbacks
{
    public static NetworkHelper Instance;
    private Player[] PlayerList;
    public GameTexts GameTexts;

    [ComponentInject] private PhotonView photonView;

    void Awake()
    {
        Instance = this;
        this.ComponentInject();
    }   

    public void SetGameText(string gameText, bool network)
    {
        if (network)
        {
            photonView.RPC("RPC_SetGameText", RpcTarget.All, gameText);
        }
        else
        {
            GameTexts.GameText.text = gameText;
        }        
    }

    [PunRPC]
    public void RPC_SetGameText(string gameText)
    {
        GameTexts.GameText.text = gameText;
    }

    public void SetActionText(string actionText, bool network)
    {
        if(network)
        {
            photonView.RPC("RPC_SetActionText", RpcTarget.All, actionText);
        }
        else
        {
            GameTexts.ActionText.text = actionText;
        }        
    }

    [PunRPC]
    public void RPC_SetActionText(string actionText)
    {
        GameTexts.ActionText.text = actionText;
    }

    private void Start()
    {
        PlayerList = PhotonNetwork.PlayerList;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        PlayerList = PhotonNetwork.PlayerList;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PlayerList = PhotonNetwork.PlayerList;
    }

    public void RefreshPlayerList()
    {
        PlayerList = PhotonNetwork.PlayerList;
    }


    public Player[] GetPlayerList() => PlayerList;

    public List<PlayerScript> GetPlayers()
    {
        return GameObject.FindGameObjectsWithTag(Statics.TAG_PLAYER).Select(x => x.GetComponent<PlayerScript>()).ToList();
    }

    public List<PlayerScript> GetOtherPlayersScript(PlayerScript self)
    {
        return GetPlayers().Where(x => x != self).ToList();
    }

    public PlayerScript GetMyPlayerScript() => GetPlayerScript(PhotonNetwork.LocalPlayer.ActorNumber);

    public Player GetPlayerNetworkByActorNr(int actorNr) => PlayerList.FirstOrDefault(x => x.ActorNumber == actorNr);
    public PlayerScript GetPlayerScriptById(int id) => GetPlayers().FirstOrDefault(x => x.PlayerId == id);

    public PlayerScript GetPlayerScript(int actorNr)
    {
        if (!PhotonNetwork.IsConnected)
        {
            return null;
        }
        var playerGos = GameObject.FindGameObjectsWithTag(Statics.TAG_PLAYER);
        foreach (var playerGo in playerGos)
        {
            var photonview = playerGo.GetComponent<PhotonView>();
            if(photonview != null && photonview.OwnerActorNr == actorNr)
            {
                return playerGo.GetComponent<PlayerScript>();
            }
        }

        return null;
    }

    public void PunDestroyAfterXSeconds(GameObject go, float secondsToDestroy)
    {
        StartCoroutine(CR_PunDestroyAfterXSeconds(go, secondsToDestroy));
    }

    private IEnumerator CR_PunDestroyAfterXSeconds(GameObject go, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        PhotonNetwork.Destroy(go);
    }
}