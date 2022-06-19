using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEditor;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public float BufferSizeforCameraRange;

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        Vector2 topRight = MonoHelper.Instance.GetTopRightOfMainCam();

        var maxX = topRight.x - BufferSizeforCameraRange;
        var minX = -maxX;

        var y = -4f;

        var randomPos = new Vector2(Random.Range(minX, maxX), y);
        var player = PhotonNetwork.Instantiate(PlayerPrefab.name, randomPos, Quaternion.identity);
    }
}