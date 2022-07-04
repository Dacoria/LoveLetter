using UnityEngine;

public class HideShowGoScript : MonoBehaviour
{
    public ScrollRulesScript ScrollRulesScript;
    public ScoreScript ScoreScript;

    public void ShowScrollRules()
    {
        ScrollRulesScript.gameObject.SetActive(true);
    }

    public void ShowScore()
    {
        ScoreScript.gameObject.SetActive(true);
    }

    public void HideScore()
    {
        ScoreScript.gameObject.SetActive(false);
    }
}
