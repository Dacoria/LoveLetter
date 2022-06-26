
using UnityEngine;
using TMPro;

public class ModalOptionScript : MonoBehaviour
{
    public TMP_Text Text;
    [HideInInspector] public ModalScript ModalScript;        

    public void ClickOnModalOption()
    {
        ModalScript.SelectOption(Text.text);
    }
}