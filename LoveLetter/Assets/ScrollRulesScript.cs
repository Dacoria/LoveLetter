using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRulesScript : MonoBehaviour
{
    public List<Sprite> InstructionSprites;
    public GameObject AllImages;
    public GameObject ScrollContainer;
    public Image Image1;
    public Image Image2;
    public Image Image3;
    private CanvasGroup canvasGroupScroll;
    public Transform ScrollTargetDown;
    public Transform ScrollTargetUp;

    public int CurrentImageIndex;

    private void Awake()
    {
        startPosition = AllImages.transform.position;
        canvasGroupScroll = ScrollContainer.GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        CurrentImageIndex = 0;
        UpdateNewState();
        if(canvasGroupScroll != null)
        {
            StartCoroutine(StartShowingScroll());
        }
        
    }

    private IEnumerator StartShowingScroll()
    {
        canvasGroupScroll.blocksRaycasts = false;
        canvasGroupScroll.interactable = false;

        
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            canvasGroupScroll.alpha = i;
            yield return null;
        }

        canvasGroupScroll.blocksRaycasts = true;
        canvasGroupScroll.interactable = true;
    }

    public void NextPage()
    {
        if(!LerpIsActive && CurrentImageIndex < InstructionSprites.Count -1)
        {
            endPosition = new Vector2(AllImages.transform.position.x, ScrollTargetDown.position.y);
            CurrentImageIndex++;
            LerpIsActive = true;
            canvasGroupScroll.interactable = false;
        }
    }

    public void PreviousPage()
    {
        if (!LerpIsActive && CurrentImageIndex > 0)
        {
            endPosition = new Vector2(AllImages.transform.position.x, ScrollTargetUp.position.y);
            CurrentImageIndex--;
            LerpIsActive = true;
            canvasGroupScroll.interactable = false;
        }
    }

    public bool LerpIsActive;

    private float elapsedTime;
    private float desiredDuration = 1.5f;

    public AnimationCurve curve;
    private Vector2 startPosition;
    private Vector2 endPosition;

    void Update()
    {
        if (!LerpIsActive)
        {
            return;
        }

        if (elapsedTime > desiredDuration)
        {
            UpdateNewState();
            return;
        }

        elapsedTime += Time.deltaTime;
        float percComplete = elapsedTime / desiredDuration;

        AllImages.transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percComplete));
    }

    private void UpdateNewState()
    {
        LerpIsActive = false;
        canvasGroupScroll.interactable = true;
        elapsedTime = 0;

        AllImages.transform.position = startPosition;
        Image1.sprite = InstructionSprites[Math.Max(0, CurrentImageIndex - 1)];
        Image2.sprite = InstructionSprites[CurrentImageIndex];
        Image3.sprite = InstructionSprites[Math.Min(InstructionSprites.Count - 1, CurrentImageIndex + 1)];
    }

    public void CloseScroll()
    {
        gameObject.SetActive(false);
    }
}
