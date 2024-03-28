using DG.Tweening;
using UnityEngine;

public class GrowAnimation : MonoBehaviour
{
    [SerializeField] Transform ObjectTransform;

    [Header("Animation Settings")]
    [SerializeField] float scaleDuration = 0.7f;
    [SerializeField] float targetScale = 1f;
    [SerializeField] Ease ease = Ease.OutBack;
    public void StartAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(targetScale, scaleDuration).SetEase(ease);
    }
}
