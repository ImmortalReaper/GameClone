using DG.Tweening;
using UnityEngine;

public class CollectResourceUIAnimation : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] CanvasGroup canvasGroup;

    [Header("First Scale")]
    [SerializeField] float firstTargetScale = 1.2f;
    [SerializeField] float firstScaleDuration = 0.2f;
    [SerializeField] Ease firstEase = Ease.OutQuint;

    [Header("Second Scale")]
    [SerializeField] float secondTargetScale = 1f;
    [SerializeField] float secondScaleDuration = 0.2f;
    [SerializeField] Ease secondEase = Ease.OutQuint;

    [Header("Fade")]
    [SerializeField] float targetFade = 0f;
    [SerializeField] float fadeDuration = 0.2f;
    [SerializeField] Ease fadeEase = Ease.InQuint;

    public void StartAnimation()
    {
        canvasGroup.alpha = 1f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasTransform.DOScale(firstTargetScale, firstScaleDuration).SetEase(firstEase))
                .Append(canvasTransform.DOScale(secondTargetScale, secondScaleDuration).SetEase(secondEase))
                .Append(canvasGroup.DOFade(targetFade, fadeDuration).SetEase(fadeEase));
    }
}
