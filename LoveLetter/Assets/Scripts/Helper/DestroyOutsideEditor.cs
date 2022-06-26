using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutsideEditor : MonoBehaviour
{
    void Start()
    {
        var destoyGo = true;
        #if UNITY_EDITOR
            destoyGo = false;
        #endif

        if(destoyGo)
        {
            Destroy(gameObject);
        }
    }
}