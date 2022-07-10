using UnityEngine;

public class HideShowGoScript : MonoBehaviour
{
    public ScrollRulesScript ScrollRulesScript;
    public ScoreScript ScoreScript;
    public DialogText DialogText;

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

    public void ShowDialogText()
    {
        DialogText.gameObject.SetActive(true);
    }

    public void HideDialogText()
    {
        DialogText.gameObject.SetActive(false);
    }
}
