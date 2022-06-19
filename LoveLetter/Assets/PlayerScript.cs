using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerScript : MonoBehaviour
{
    [ComponentInject] private TMP_Text PlayerText;

    private void Awake()
    {
        this.ComponentInject();
        PlayerText.text = PhotonNetwork.NickName;
    }    
}
