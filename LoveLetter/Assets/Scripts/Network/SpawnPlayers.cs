using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public float BufferSizeforCameraRange;
    public bool UseAiInDummy;

    private void Start()
    {
        
        SpawnPlayer("P" + GetPlayerCounter(), false);
    }

    private int GetPlayerCounter()
    {
        var currentPlayers = NetworkHelper.Instance.GetPlayerList();
        var currentGoObjects = NetworkHelper.Instance.GetPlayers();

        var playerCount = Mathf.Max(currentPlayers.Length, currentGoObjects.Count);
        return playerCount;
    }

    public void SpawnDummyPlayer()
    {        
        var playerCounter = GetPlayerCounter();
        if (playerCounter == 1)
        {
            SpawnPlayer("AI2", true);
        }
        else if(playerCounter == 2)
        {
            SpawnPlayer("AI3", true);
        }
        else if (playerCounter == 3)
        {
            SpawnPlayer("AI4", true);
        }
    }    

    public void SpawnPlayer(string name, bool isDummy)
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        var isAi = isDummy && !PhotonNetwork.OfflineMode; // offline = altijd dummy zonder AI

        if (!PhotonNetwork.OfflineMode && !isAi)
        {
            name = PhotonNetwork.NickName;
        }

        var counterId = NetworkHelper.Instance.GetPlayers().Count(); // aantal spawned Playerobject (inclusief jijzelf)
        object[] myCustomInitData = new List<object> { name, isAi, counterId }.ToArray();
        var player = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(0,0), Quaternion.identity, 0, myCustomInitData);        
    }
}