using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public ConnectMethod ConnectMethod;

    public void StartGameOnlineFast()
    {
        StartGame(ConnectMethod.Online_Fast);
    }

    public void StartGameOnlineManualSetup()
    {
        StartGame(ConnectMethod.Online_Manual_Setup);
    }

    public void StartGameOffline()
    {
        StartGame(ConnectMethod.Offline);
    }

    public void StartGame(ConnectMethod connectMethod)
    {
        ConnectMethod = connectMethod;
        Debug.Log("StartGame " + connectMethod);
        PhotonNetwork.ConnectUsingSettings(PhotonNetwork.PhotonServerSettings.AppSettings, startInOfflineMode: connectMethod == ConnectMethod.Offline);
    }   


    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        if(ConnectMethod == ConnectMethod.Offline)
        {
            PhotonNetwork.JoinRandomRoom();
            LevelLoader.instance.LoadScene(Statics.SCENE_LEVEL1, false);
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }        
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        

        if (ConnectMethod == ConnectMethod.Online_Fast)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions
            {
                MaxPlayers = 4
            });
        }
        else if (ConnectMethod == ConnectMethod.Online_Manual_Setup)
        {
            LevelLoader.instance.LoadScene(Statics.SCENE_LOBBY, false);
        }
    }    

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (ConnectMethod == ConnectMethod.Online_Fast)
        {
            PhotonNetwork.NickName = PhotonNetwork.IsMasterClient ? "Host" : "Client";
        }
        LevelLoader.instance.LoadScene(Statics.SCENE_LOBBY, true);
    }
}

[SerializeField]
public enum ConnectMethod
{
    Online_Fast,
    Online_Manual_Setup,
    Offline
}