
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModalOptionScript : MonoBehaviour
{
    public TMP_Text Text;
    [HideInInspector] public ModalScript ModalScript;

    public GameObject GridLayoutCircleCount;
    public Image CircleCountPrefab;

    public void ClickOnModalOption()
    {
        ModalScript.SelectOption(Text.text);
    }

    public void SetCountInDeck(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(CircleCountPrefab, GridLayoutCircleCount.transform);
        }
    }
}