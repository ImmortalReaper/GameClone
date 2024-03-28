using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneAnimationController : MonoBehaviour
{
    [Header("Shrink Animation")]
    [SerializeField] Transform rootTransform;
    public Transform RootTransform
    {
        get { return rootTransform; }
        set { rootTransform = value; }
    }
    [SerializeField] SpriteRenderer zoneSpriteRenderer;
    [SerializeField] Transform zoneCanvas;

    [Header("Shrink Animation Settings")]
    [SerializeField] float targetScale = 0;
    [SerializeField] float scaleDuration = 0.5f;
    [SerializeField] Ease ease = Ease.InOutBack;

    [Header("Player Enter Zone Animation Settings")]
    [SerializeField] float enterTargetScale = 1.2f;
    [SerializeField] float enterScaleDuration = 0.2f;

    [Header("Player Exit Zone Animation Settings")]
    [SerializeField] float exitTargetScale = 1;
    [SerializeField] float exitScaleDuration = 0.2f;

    BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void ShrinkAnimation()
    {
        boxCollider.enabled = false;
        zoneCanvas.gameObject.SetActive(false);
        zoneSpriteRenderer.transform.DOScale(targetScale, scaleDuration)
                                    .SetEase(ease)
                                    .OnComplete(() => rootTransform.gameObject.SetActive(false));
    }

    void OnTriggerEnter(Collider other)
    {
        zoneSpriteRenderer.transform.DOScale(enterTargetScale, enterScaleDuration);
    }

    void OnTriggerExit(Collider other)
    {
        zoneSpriteRenderer.transform.DOScale(exitTargetScale, exitScaleDuration);
    }
}
