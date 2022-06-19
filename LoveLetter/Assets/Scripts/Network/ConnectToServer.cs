using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField levelText;
    public TMP_InputField NickName;

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
            SceneManager.LoadScene(Statics.SCENE_LEVEL1);
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
            SceneManager.LoadScene(Statics.SCENE_LOBBY);
        }
    }    

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (ConnectMethod == ConnectMethod.Online_Fast)
        {
            if (NickName != null && !string.IsNullOrEmpty(NickName.text))
            {
                string name = NickName.text;
                if (NickName.text.Length > 10)
                {
                    name = NickName.text.Substring(0, 10);
                }

                PhotonNetwork.NickName = name.TrimEnd();

                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.NickName = PhotonNetwork.NickName + " (host)";
                }
            }
            else
            {
                PhotonNetwork.NickName = PhotonNetwork.IsMasterClient ? "Host" : "Client";
            }
        }
        if (levelText != null && !string.IsNullOrEmpty(levelText.text))
        {
            PhotonNetwork.LoadLevel(levelText.text);
        }
        else
        {
            PhotonNetwork.LoadLevel(Statics.SCENE_LEVEL1);
        }
    }
}

[SerializeField]
public enum ConnectMethod
{
    Online_Fast,
    Online_Manual_Setup,
    Offline
}