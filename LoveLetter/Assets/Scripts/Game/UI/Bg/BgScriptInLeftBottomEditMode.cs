using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BgScriptInLeftBottomEditMode : MonoBehaviour
{
    public Image Image;

    private void Start()
    {
        DestroyImmediate(this);
    }

    void Update()
    {
        var leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        var rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        var downLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        var topLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        Image.transform.position = new Vector3(leftLimit, downLimit);
    }
}
