using System;
using System.Collections.Generic;
using UnityEngine;

public class ModalScript : MonoBehaviour
{
    public ModalOptionScript ModalOptionPrefab;
    public GameObject ModalOptions;
    public TMPro.TMP_Text HeaderModal;
  

    private Action<string> Callback;

    private void Awake()
    {
        ShowHideDirectChildren(false);
    }

    public void SetOptions(Action<string> callback, string title, List<string> options)
    {
        Callback = callback;

        HeaderModal.text = title;
        ShowHideDirectChildren(true);

        foreach (string option in options)
        {
            var modalOptionGo = Instantiate(ModalOptionPrefab, ModalOptions.transform);
            var modalOptionScript = modalOptionGo.GetComponent<ModalOptionScript>();
            modalOptionScript.Text.text = option;
            modalOptionScript.ModalScript = this;
        }
    }

    public void SelectOption(string optionText)
    {
        HideModal();
        Callback(optionText);
    }   

    private void HideModal()
    {
        for (int i = ModalOptions.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(ModalOptions.transform.GetChild(i).gameObject);
        }

        ShowHideDirectChildren(false);
    }

    public bool IsActive;

    private void ShowHideDirectChildren(bool active)
    {
        IsActive = active;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
        ModalOptions.SetActive(active);
    }
}