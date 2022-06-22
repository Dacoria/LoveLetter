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
    private GameTexts GameTexts;

    [ComponentInject] private PhotonView photonView;

    void Awake()
    {
        Instance = this;
        this.ComponentInject();
        GameTexts = FindObjectOfType<GameTexts>();
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

    public List<PlayerScript> GetOtherPlayers(PlayerScript self)
    {
        return GetPlayers().Where(x => x != self).ToList();
    }

    public GameObject GetMyPlayerGo() => GetPlayerGo(PhotonNetwork.LocalPlayer.ActorNumber);

    public Player GetPlayerByActorNr(int actorNr) => PlayerList.FirstOrDefault(x => x.ActorNumber == actorNr);
    public PlayerScript GetPlayerById(int id) => GetPlayers().FirstOrDefault(x => x.PlayerId == id);

    public GameObject GetPlayerGo(int actorNr)
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
                return playerGo;
            }
        }

        return null;
    }

    public GameObject GetClosestPlayerGo(Transform myLoc)
    {
        if (!PhotonNetwork.IsConnected)
        {
            return null;
        }

        var playerGos = GameObject.FindGameObjectsWithTag(Statics.TAG_PLAYER);
        return playerGos.OrderBy(playerLoc => Vector3.Distance(playerLoc.transform.position, myLoc.position)).FirstOrDefault();
    }  

    public void PunDestroyAfterXSeconds(GameObject go, float secondsToDestroy)
    {
        StartCoroutine(CR_PunDestroyAfterXSeconds(go, secondsToDestroy));
    }

    public IEnumerator CR_PunDestroyAfterXSeconds(GameObject go, float secondsToDestroy)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        PhotonNetwork.Destroy(go);
    }
}