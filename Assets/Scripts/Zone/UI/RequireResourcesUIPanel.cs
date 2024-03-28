using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequireResourcesUIPanel : MonoBehaviour
{
    public ResourceType resourceType;

    [Header("UI Elements")]
    [SerializeField] Image resourceImage;
    [SerializeField] TextMeshProUGUI depositResourceText;
    [SerializeField] TextMeshProUGUI requireResourceText;
    [SerializeField] int depositResource = 0;
    [SerializeField] int requireResource = 0;

    void Start()
    {
        resourceImage.sprite = resourceType.resourceSprite;
    }

    public void SetRequireResourceAmount(int amount)
    {
        requireResource = amount;
        requireResourceText.text = $"/{amount}";
    }

    public void AddDepositResource(int amount)
    {
        depositResource += amount;
        depositResourceText.text = depositResource.ToString();
    }

    public bool IsResourceComplete()
    {
        if (requireResource == depositResource) { return true; }
        return false;
    }
}
