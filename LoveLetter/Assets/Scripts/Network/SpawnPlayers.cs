using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public float BufferSizeforCameraRange;

    private void Start()
    {
        var currentPlayers = NetworkHelper.Instance.GetPlayerList();

        var playerCount = currentPlayers.Length;
        SpawnPlayer("P" + playerCount, GetPos(playerCount));
    }

    private int playerCounter;

    public void SpawnDummyPlayer()
    {        
        
        if (playerCounter == 1)
        {
            SpawnPlayer("DP2", GetPos(2));
        }
        else if(playerCounter == 2)
        {
            SpawnPlayer("DP3", GetPos(3));
        }
        else if (playerCounter == 3)
        {
            SpawnPlayer("DP4", GetPos(4));
        }
    }

    private Vector2 GetPos(int playerIndex)
    {
        Vector2 topRight = MonoHelper.Instance.GetTopRightOfMainCam();
        if (playerIndex == 1)
        {
            return new Vector2(topRight.x / 4 * 1.5f * -1, topRight.y / 2);
        }
        if (playerIndex == 2)
        {
            return new Vector2(topRight.x / 4 * 1.5f, topRight.y / 2);
        }
        if (playerIndex == 3)
        {
            return new Vector2(topRight.x / 4 * 1.5f * -1, 0);
        }
        if (playerIndex == 4)
        {
            return new Vector2(topRight.x / 4 * 1.5f, 0);
        }

        throw new System.Exception();

    }

    public void SpawnPlayer(string name, Vector2 pos)
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        playerCounter++;

        if (!PhotonNetwork.OfflineMode)
        {
            name = PhotonNetwork.NickName;
        }

        object[] myCustomInitData = new List<object> { name, playerCounter }.ToArray();
        var player = PhotonNetwork.Instantiate(PlayerPrefab.name, pos, Quaternion.identity, 0, myCustomInitData);        
    }
}