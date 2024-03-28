using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanelUI : MonoBehaviour
{
    [SerializeField] ResourceType resourceType;
    [SerializeField] Image resourceImage;
    [SerializeField] TextMeshProUGUI resourceAmount;

    [Header("First Scale Animation Settings")]
    [SerializeField] float firstTargetScale = 1.2f;
    [SerializeField] float firstScaleDuration = 0.1f;

    [Header("Second Scale Animation Settings")]
    [SerializeField] float secongTargetScale = 1f;
    [SerializeField] float secondScaleDuration = 0.1f;

    public ResourceType ResourceType
    {
        get { return resourceType; }
        private set { resourceType = value; }
    }

    void Start()
    {
        if (resourceType != null) { SetResourceImage(resourceType.resourceSprite); }
    }

    public void SetResourceType(ResourceType type)
    {
        resourceType = type;
        SetResourceImage(type.resourceSprite);
    }

    public void SetResourceImage(Sprite image)
    {
        resourceImage.sprite = image;
    }

    public void SetResourceAmount(int amount)
    {
        resourceAmount.text = amount.ToString();
        ResourceAmountChangeAnimation();
    }

    void ResourceAmountChangeAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(resourceImage.transform.DOScale(firstTargetScale, firstScaleDuration))
                .Append(resourceImage.transform.DOScale(secongTargetScale, secondScaleDuration));
    }
}
