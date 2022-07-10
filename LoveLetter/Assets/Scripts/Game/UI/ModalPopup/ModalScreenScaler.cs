using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ModalScreenScaler : MonoBehaviour
{
    public float LocalScaleMultiplier = 1f;

    private Vector3 InitScale = new Vector3(1, 1, 1);
    
    void Update()
    {
        if(Screen.width > Screen.height * 2)
        {
            transform.localScale = InitScale * 0.45f;
        }
        else if (Screen.width > Screen.height * 1.7)
        {
            transform.localScale = InitScale * 0.5f;
        }
        else if (Screen.width > Screen.height * 1.4)
        {
            transform.localScale = InitScale * 0.6f;
        }
        else if (Screen.width > Screen.height * 1.2)
        {
            transform.localScale = InitScale * 0.65f;
        }
        else if (Screen.width > Screen.height * 1)
        {
            transform.localScale = InitScale * 0.7f;
        }
        else if (Screen.width > Screen.height * 0.8)
        {
            transform.localScale = InitScale * 0.75f;
        }
        else
        {
            transform.localScale = InitScale * 0.85f;
        }
    }
}
