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

    [SerializeField] private AnimationCurve curve;

    private bool isActive;

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
        this.localScaleTarget = new Vector2(localScaleMultiplier, localScaleMultiplier);
        this.desiredDuration = desiredDuration;

        elapsedTime = 0;
        isActive = true;
    }

    void Update()
    {
        if(!isActive)
        {
            return;
        }

        if (elapsedTime > desiredDuration)
        {
            isActive = false;
            if (destroyAfterDestReached)
            {
                Destroy(gameObject);
            }
            return;
        }

        elapsedTime += Time.deltaTime;
        float percComplete = elapsedTime / desiredDuration;

        transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percComplete));
        transform.localScale = Vector2.Lerp(startScale, localScaleTarget, percComplete);
    }
}