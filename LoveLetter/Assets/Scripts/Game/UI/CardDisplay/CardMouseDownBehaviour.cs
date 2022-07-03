using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMouseDownBehaviour : MonoBehaviour
{
    [ComponentInject] private IOnCardMouseDownEvent OnCardMouseDownEvent;

    private void Awake()
    {
        this.ComponentInject();
    }

    void OnMouseDown()
    {
        if(MonoHelper.Instance.GuiAllowed())
        {
            OnCardMouseDownEvent.OnCardMouseDownEvent();
        }        
    }
}
