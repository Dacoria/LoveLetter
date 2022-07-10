using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ModalScreenScaler : MonoBehaviour
{
    public float LocalScaleMultiplier = 1f;

    private Vector3 InitScale = new Vector3(1, 1, 1);

    private RectTransform RectTransform;
    private void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(Screen.width > Screen.height * 2)
        {
            transform.localScale = InitScale * 0.35f;
        }
        else if (Screen.width > Screen.height * 1.7)
        {
            transform.localScale = InitScale * 0.4f;
        }
        else if (Screen.width > Screen.height * 1.4)
        {
            transform.localScale = InitScale * 0.45f;
        }
        else if (Screen.width > Screen.height * 1.1)
        {
            transform.localScale = InitScale * 0.55f;
        }
        else if (Screen.width > Screen.height * 0.9)
        {
            transform.localScale = InitScale * 0.65f;
        }
        else if (Screen.width > Screen.height * 0.75)
        {
            transform.localScale = InitScale * 0.7f;
        }
        else
        {
            transform.localScale = InitScale * 0.75f;
        }

        var rectWidth = RectTransform.rect.width;

        if(rectWidth > Screen.width)
        {
            transform.localScale *= Screen.width / rectWidth * 0.95f;
        }

    }
}
