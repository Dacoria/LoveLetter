using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicker : MonoBehaviour
{
    public void OnButtonClickSetNogInteractable(Button btn)
    {
        btn.interactable = false;
    }
}
