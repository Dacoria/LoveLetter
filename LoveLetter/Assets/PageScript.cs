using UnityEngine;
using UnityEngine.UI;

public class PageScript : MonoBehaviour
{
    [ComponentInject] private ScrollRulesScript scrollRulesScript;
    [ComponentInject] private Button button;

    public PageType PageType;

    void Awake()
    {
        this.ComponentInject();    
    }

    private void Start()
    {
        UpdateButtonIsActive(true);
    }

    private bool previousStatusButtonIsActive;
    void Update()
    {
        var buttonIsActive = GetButtonIsActiveStatus();
        if (previousStatusButtonIsActive != buttonIsActive)
        {
            UpdateButtonIsActive(buttonIsActive);
        }
    }

    private void UpdateButtonIsActive(bool buttonIsActive)
    {
        button.interactable = buttonIsActive;
        previousStatusButtonIsActive = buttonIsActive;
    }

    private bool GetButtonIsActiveStatus()
    {
        if(PageType == PageType.Previous)
        {
            return scrollRulesScript.CurrentImageIndex != 0;
        }
        else if(PageType == PageType.Next)
        {
            return scrollRulesScript.CurrentImageIndex < scrollRulesScript.InstructionSprites.Count - 1;
        }

        throw new System.Exception("");
    }
}

public enum PageType
{
    Previous,
    Next
}