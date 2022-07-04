using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text TitleText;
    public GameObject Players;
    public PlayerScoreScript PlayerScorePrefab;

    [ComponentInject] private CanvasGroup canvasGroup;

    private void Awake()
    {
        this.ComponentInject();
    }

    public void OnEnable()
    {
        Reset();
        var players = NetworkHelper.Instance.GetPlayers().OrderByDescending(x => x.Score).ThenBy(x => x.PlayerId).ToList();

        TitleText.text = "Score (First to " + MonoHelper.Instance.GetRoseCountToWinGame(players.Count).ToString() + ")";

        var counter = 0;
        foreach(var player in players)
        {
            var playerGo = Instantiate(PlayerScorePrefab, Players.transform);
            playerGo.PlayerText.text = player.PlayerName;
            playerGo.EmotionImage.sprite = MonoHelper.Instance.GetEmoticonSprite(counter);
            
            playerGo.SetScore(player.Score);
            counter++;
        }

        StartCoroutine(StartShowingScore());
    }



    private IEnumerator StartShowingScore()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;


        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            canvasGroup.alpha = i;
            yield return null;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void Reset()
    {
        for (int i = Players.transform.childCount - 1; i >= 0; i-- )
        {
            Destroy(Players.transform.GetChild(i).gameObject);
        }
    }
}
