using DG.Tweening;
using UnityEngine;

public class DamageResourceAnimation : MonoBehaviour
{
    [SerializeField] ResourceController resourceController;

    [Header("First Scale Settings")]
    [SerializeField] float firstTargetScale = 0.8f;
    [SerializeField] float firstScaleDuration = 0.1f;
    [SerializeField] Ease firstEase = Ease.OutQuint;

    [Header("Second Scale Settings")]
    [SerializeField] float secondTargetScale = 1f;
    [SerializeField] float secondScaleDuration = 0.1f;
    [SerializeField] Ease secondEase = Ease.OutQuint;

    void OnEnable()
    {
        resourceController.OnResourceDamage += StartAnimation;
    }

    void OnDestroy()
    {
        resourceController.OnResourceDamage -= StartAnimation;
    }

    public void StartAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(firstTargetScale, firstScaleDuration).SetEase(firstEase))
                .Append(transform.DOScale(secondTargetScale, secondScaleDuration).SetEase(secondEase));
    }
}
