using System;
using System.Collections.Generic;
using UnityEngine;

public class BigCardHandler : MonoBehaviour
{
    public static BigCardHandler instance;
    public BigCardDisplay BigCardDisplay;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ActionEvents.NewPlayerTurn += OnNewPlayerTurn;
        ActionEvents.RoundEnded += OnRoundEnded;
        ActionEvents.NewRoundStarted += OnNewRoundStarted;
        ActionEvents.CardDiscarded += OnCardDiscarded;
        ActionEvents.CardsSwitched += OnCardsSwitched;
        ActionEvents.CardsToDeck += OnCardsToDeck;
        ActionEvents.EndCharacterEffect += OnEndCharacterEffect;
    }

    void OnDestroy()
    {
        ActionEvents.NewPlayerTurn -= OnNewPlayerTurn;
        ActionEvents.RoundEnded -= OnRoundEnded;
        ActionEvents.NewRoundStarted -= OnNewRoundStarted;
        ActionEvents.CardDiscarded -= OnCardDiscarded;
        ActionEvents.CardsSwitched -= OnCardsSwitched;
        ActionEvents.CardsToDeck -= OnCardsToDeck;
        ActionEvents.EndCharacterEffect -= OnEndCharacterEffect;
    }

    public bool BigCardIsActive()
    {
        return BigCardDisplay.isActiveAndEnabled && BigCardDisplay.BigCardIsActive;
    }

    private bool BigCardCanBeShowed()
    {
        if(!MonoHelper.Instance.GuiAllowed(
            checkOptionsModal: false
            ))
        {
            return false;
        }

        if (timeWaited < targetTime)
        {
            return false;
        }

        return true;
    }

    public void ShowBigCardNoButtons(CharacterType type)
    {
        if (!BigCardCanBeShowed())
        {
            return;
        }

        Debug.Log(timeWaited + " " + targetTime);

        BigCardDisplay.gameObject.SetActive(true);
        BigCardDisplay.ShowBigCard(type, -1, -1, ignoreModalActive: true);
    }

    public void ShowBigCardWithButtons(CharacterType type, int cardId, int playerId)
    {
        if (!BigCardCanBeShowed())
        {
            return;
        }

        BigCardDisplay.gameObject.SetActive(true);
        BigCardDisplay.ShowBigCard(type, cardId, playerId, ignoreModalActive: true);
    }

    private float timeWaited;
    private float targetTime;

    private void Update()
    {
        timeWaited += Time.deltaTime;
    }

    private void OnCardsToDeck(List<int> obj)
    {
        SetNewWaitTime(2.4f);
    }

    private void OnCardsSwitched(int arg1, int arg2)
    {
        SetNewWaitTime(2.5f);
    }

    private void OnCardDiscarded(int obj)
    {
        SetNewWaitTime(4);
    }

    private void OnNewRoundStarted(List<int> arg1, int arg2)
    {
        SetNewWaitTime(3, resetTimer: true);
    }

    private void OnRoundEnded(RoundEnded roundEnded)
    {
        SetNewWaitTime(0.9f, resetTimer: true);
    }

    private void OnNewPlayerTurn(int obj)
    {
        SetNewWaitTime(1, resetTimer: true);
    }

    private void OnEndCharacterEffect(int arg1, CharacterType arg2, int arg3)
    {
        SetNewWaitTime(20); // gereset door new turn event
    }

    private void SetNewWaitTime(float newWaitTime, bool resetTimer = false)
    {
        var timeRemainingToWait = targetTime < timeWaited ? 0 : timeWaited - targetTime;

        if(newWaitTime > timeRemainingToWait)
        {
            timeWaited = 0;
            targetTime = newWaitTime; 
        }
    }
}
