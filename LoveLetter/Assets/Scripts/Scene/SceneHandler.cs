using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public bool DebugOffline = true;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(Statics.SCENE_LOADING);
        }
    }
}
