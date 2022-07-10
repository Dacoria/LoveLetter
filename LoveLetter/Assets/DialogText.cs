using UnityEngine;
using TMPro;

public class DialogText : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text Description;
    public GameObject ScoreButton;

    public void SetText(string title, string description, bool showScore)
    {
        Title.text = title;
        Description.text = description;
        ScoreButton.SetActive(showScore);
    }    
}