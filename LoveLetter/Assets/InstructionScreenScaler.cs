using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InstructionScreenScaler : MonoBehaviour
{
    private Vector3 InitLocalScale = new Vector3(0.5f, 0.5f, 1);
    public RectTransform rect;

    private void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Screen.height > Screen.width)
        {
            transform.localScale = InitLocalScale;
        }
        else
        {
            var downLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
            var topLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

            Debug.Log(Screen.height + "  " + Screen.width + downLimit + "  " + topLimit + "  " + rect.sizeDelta.y + "  " + rect.rect.height);
            //transform.localScale = new Vector3(0.3f, 0.3f, 1);

        }

    }
}
