using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ResourceParticalAnimation : MonoBehaviour
{
    [SerializeField] ResourceType resourceType;

    [Header("Spawn Animation Settings")]
    [SerializeField] float freeFlyDuration = 1.0f;
    [SerializeField] float speedProjectile = 3f;
    [SerializeField] float maxAngleDeviation = 40f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float maxRotationAngle = 2000f;

    [Header("To target position animation settings")]
    [SerializeField] float moveDuration = 0.3f;
    [SerializeField] float targetScale = 0;
    [SerializeField] float scaleDuration = 0.3f;

    public Action<ResourceType, ResourceParticalAnimation> OnAnimationEnd;
    public ObjectPool<GameObject> Pool;

    Vector3 particleEmitDirection;
    Sequence sequence;

    public void StartAnimation(Vector3 spawnPosition, Vector3 targetPosition, Vector3 targetOffset, Vector3 launchDirection)
    {
        transform.position = spawnPosition;

        float randomAngleX = Random.Range(-maxAngleDeviation, maxAngleDeviation);
        float randomAngleY = Random.Range(-maxAngleDeviation, maxAngleDeviation);
        Quaternion randomRotation = Quaternion.Euler(randomAngleX, randomAngleY, 0f);
        particleEmitDirection = randomRotation * launchDirection.normalized;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(transform.position + particleEmitDirection * speedProjectile, freeFlyDuration)
                .SetEase(Ease.OutQuart)
                .OnStart(() =>
                {
                    Vector3 mult = new Vector3(Random.value, Random.value, Random.value);
                    transform.DORotate(mult * maxRotationAngle, rotationSpeed, RotateMode.FastBeyond360)
                        .SetRelative()
                        .SetEase(Ease.Linear)
                        .SetLoops(-1)
                        .SetId(gameObject);
                }))
                .Append(transform.DOMove(targetPosition + targetOffset, moveDuration).SetEase(Ease.InQuart))
                .Join(transform.DOScale(targetScale, scaleDuration).SetEase(Ease.InQuart))
                .OnComplete(DestroyObject);
    }

    void DestroyObject()
    {
        DOTween.Kill(gameObject);
        sequence.Kill();
        OnAnimationEnd?.Invoke(resourceType, this);
        Destroy(gameObject);
    }
}
