using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField NameInputField;

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
        if (ConnectMethod == ConnectMethod.Offline)
        {
            PhotonNetwork.JoinRandomRoom();
            LevelLoader.instance.LoadScene(Statics.SCENE_LEVEL1);
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
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        else if (ConnectMethod == ConnectMethod.Online_Manual_Setup)
        {
            LevelLoader.instance.LoadScene(Statics.SCENE_LOBBY);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        if (ConnectMethod == ConnectMethod.Online_Fast)
        {
            if (string.IsNullOrEmpty(NameInputField.text))
            {
                PhotonNetwork.NickName = PhotonNetwork.IsMasterClient ? "Host" : "Client";
            }
            else
            {
                PhotonNetwork.NickName = NameInputField.text;
            }
            
        }        

        PhotonNetwork.LoadLevel(Statics.SCENE_LEVEL1);

        //sceneNamePun = Statics.SCENE_LEVEL1;
        //LevelLoader.instance.LoadSceneAnimation(PunLoadScene);
    }

   //private string sceneNamePun;
   //private void PunLoadScene()
   //{
   //    Debug.Log(1);
   //    PhotonNetwork.LoadLevel(sceneNamePun);
   //    Debug.Log(2);
   //}  

}

[SerializeField]
public enum ConnectMethod
{
    Online_Fast,
    Online_Manual_Setup,
    Offline
}