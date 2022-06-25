using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField nameInput;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            LevelLoader.instance.LoadScene(Statics.SCENE_LOADING);
            return;
        }

        if(PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.Disconnect();
            //PhotonNetwork.ConnectUsingSettings(PhotonNetwork.PhotonServerSettings.AppSettings, startInOfflineMode: false);
            LevelLoader.instance.LoadScene(Statics.SCENE_LOADING);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        PhotonNetwork.NickName = string.IsNullOrEmpty(nameInput.text) ? "unknown" : nameInput.text;
        PhotonNetwork.CreateRoom(createInput.text);
    }
   
    public void JoinRoom()
    {
        PhotonNetwork.NickName = string.IsNullOrEmpty(nameInput.text) ? "unknown" : nameInput.text;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void FastJoinRoom()
    {
        PhotonNetwork.NickName = string.IsNullOrEmpty(nameInput.text) ? (PhotonNetwork.IsMasterClient ? "Host" : "Client") : nameInput.text;
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions
        {
            MaxPlayers = 4
        });
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(Statics.SCENE_LEVEL1);
    }
}
