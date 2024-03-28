using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] CanvasGroup progressBarCanvasGroup;
    [SerializeField] Image progressBar;

    [Header("Show Progress Bar Animation Settings")]
    [SerializeField] float showProgressBarAlpha = 1;
    [SerializeField] float showDuration = 0.5f;

    [Header("Fade Progress Bar Animation Settings")]
    [SerializeField] float fadeProgressBarAlpha = 0;
    [SerializeField] float fadeDuration = 0.5f;

    public void SetProgressBarPercentage(float percentage)
    {
        progressBar.fillAmount = percentage;
    }

    public void ShowProgressBarAnimation()
    {
        progressBarCanvasGroup.DOFade(showProgressBarAlpha, showDuration);
    }

    public void FadeProgressBarAnimation()
    {
        progressBarCanvasGroup.DOFade(fadeProgressBarAlpha, fadeDuration);
    }
}
