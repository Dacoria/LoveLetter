using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public float BufferSizeforCameraRange;

    private void Start()
    {
        Vector2 topRight = MonoHelper.Instance.GetTopRightOfMainCam();
        SpawnPlayer("P1", topRight.x / 3 * 1.5f * -1, topRight.y / 2 );
    }

    private int playerCounter;

    public void SpawnDummyPlayer()
    {        
        Vector2 topRight = MonoHelper.Instance.GetTopRightOfMainCam();
        if (playerCounter == 1)
        {
            SpawnPlayer("P2", topRight.x / 3 * 1.5f, topRight.y / 2 );
        }
        else if(playerCounter == 2)
        {
            SpawnPlayer("P3", topRight.x / 3 * 1.5f * -1, 0);
        }
        else if (playerCounter == 3)
        {
            SpawnPlayer("P4", topRight.x / 3 * 1.5f, 0 * 1);
        }
    }

    public void SpawnPlayer(string name, float xDir, float yDir)
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        playerCounter++;

        object[] myCustomInitData = new List<object> { name }.ToArray();
        var player = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(xDir, yDir), Quaternion.identity, 0, myCustomInitData);        
    }
}