using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInstructionScrollScript : MonoBehaviour
{
    public ScrollRulesScript ScrollRulesScript;
    
    public void ShowScrollRules()
    {
        ScrollRulesScript.gameObject.SetActive(true);
    }
}
