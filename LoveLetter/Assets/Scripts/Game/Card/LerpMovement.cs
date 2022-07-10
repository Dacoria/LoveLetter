using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
    private Vector2 endPosition;
    private Vector2 startPosition;
    private float desiredDuration;
    private float elapsedTime;
    private Vector2 startScale;
    private Vector2 localScaleTarget;
    private bool destroyAfterDestReached;
    private bool useLocalScale;

    [SerializeField] private AnimationCurve curve;

    public bool IsActive;

    private void Awake()
    {
        this.ComponentInject();
    }

    public void StartMovement(Vector2 startPosition, Vector2 endPosition, float localScaleMultiplier = 1, bool destroyAfterDestReached = false, float desiredDuration = 1.5f)
    {
        this.startPosition = startPosition;
        this.startScale = transform.localScale;
        this.endPosition = endPosition;
        this.destroyAfterDestReached = destroyAfterDestReached;
        this.useLocalScale = localScaleMultiplier != 1;
        this.localScaleTarget = new Vector2(localScaleMultiplier, localScaleMultiplier);
        this.desiredDuration = desiredDuration;

        elapsedTime = 0;
        IsActive = true;
    }

    void Update()
    {
        if(!IsActive)
        {
            return;
        }

        if (elapsedTime > desiredDuration)
        {
            IsActive = false;
            if (destroyAfterDestReached)
            {
                Destroy(gameObject);
            }
            return;
        }

        elapsedTime += Time.deltaTime;
        float percComplete = elapsedTime / desiredDuration;

        transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percComplete));
        if (useLocalScale)
        {
            transform.localScale = Vector2.Lerp(startScale, localScaleTarget, percComplete);
        }
    }
}