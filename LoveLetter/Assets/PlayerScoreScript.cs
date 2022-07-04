using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerScoreScript : MonoBehaviour
{
    public Image EmotionImage;
    public TMP_Text PlayerText;
    public TMP_Text Score;
    public GameObject RosesCollection;
    public GameObject RosePrefab;

    public void SetScore(int score)
    {
        Score.text = score.ToString();
        for (int i = 0;i < score; i++)
        {
            Instantiate(RosePrefab, RosesCollection.transform);
        }
    }
}
