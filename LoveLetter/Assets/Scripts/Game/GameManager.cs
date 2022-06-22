
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;

    
    private List<PlayerScript> AllPlayers;
    public TMP_Text GameText;
    public List<PlayerScript> PlayersWhoDiscardedSpies;


    private void Awake()
    {
        instance = this;
        GameText.text = "Waiting for game to start";
    }
}