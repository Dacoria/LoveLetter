using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivePlayersScript : MonoBehaviour
{
    public TMP_Text Text;

    void Update()
    {
        Text.text = "Players Left: " + NetworkHelper.Instance.GetPlayers().Count(x => x.PlayerStatus != PlayerStatus.Intercepted).ToString();
    }
}
