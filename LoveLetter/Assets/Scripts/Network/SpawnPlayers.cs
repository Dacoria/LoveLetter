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

        SpawnPlayer("P1", topRight.x / 3 * 2 * -1, topRight.y / 3 * 1);
        SpawnPlayer("P2", 0, topRight.y / 3 * 1);
    }

    public void SpawnPlayer(string name, float xDir, float yDir)
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        Vector2 topRight = MonoHelper.Instance.GetTopRightOfMainCam();

        object[] myCustomInitData = new List<object> { name }.ToArray();
        var player = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(xDir, yDir), Quaternion.identity, 0, myCustomInitData);        
    }
}