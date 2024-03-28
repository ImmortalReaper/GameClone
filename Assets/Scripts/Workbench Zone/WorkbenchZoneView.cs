using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchZoneView : MonoBehaviour
{
    [SerializeField] WorkbenchZoneController workbenchZoneController;
    [SerializeField] ZoneAnimationController zoneAnimationController;

    [Header("Resource Particle System")]
    [SerializeField] ResourceParticalSpawner resourceParticalSpawner;
    [SerializeField] Transform targetTransform;

    [Header("UI Elements")]
    [SerializeField] ResourcePanelUI requireResourcesPanel;
    [SerializeField] ProgressBarUI progressBar;
    [SerializeField] Image craftingResourceImage;
    [SerializeField] TextMeshProUGUI craftingResourceAmount;

    [Header("Particle System")]
    [SerializeField] ParticleSystem depositResourceParticleSystem;

    void Start()
    {
        UIInitialization();
    }

    void OnEnable()
    {
        workbenchZoneController.OnCraftingQueueCountChange += UpdateCraftingAmountUI;
        workbenchZoneController.OnResourceDeposit += DepositResource;
        workbenchZoneController.OnWorkbenchFinishCrafting += FinishCrafting;
        workbenchZoneController.OnWorkbenchStartCrafting += StartCrafting;
    }

    void OnDestroy()
    {
        workbenchZoneController.OnCraftingQueueCountChange -= UpdateCraftingAmountUI;
        workbenchZoneController.OnResourceDeposit -= DepositResource;
        workbenchZoneController.OnWorkbenchFinishCrafting -= FinishCrafting;
        workbenchZoneController.OnWorkbenchStartCrafting -= StartCrafting;
    }

    void FixedUpdate()
    {
        progressBar.SetProgressBarPercentage(workbenchZoneController.CraftingTimer.GetCompletePercentage());
    }

    void StartCrafting()
    {
        progressBar.ShowProgressBarAnimation();
        craftingResourceAmount.enabled = true;
    }

    void FinishCrafting()
    {
        progressBar.FadeProgressBarAnimation();
        craftingResourceAmount.enabled = false;
    }

    void DepositResource(ResourceType type, int amount)
    {
        depositResourceParticleSystem.Emit(1);
        for (int i = 0; i < amount; i++)
        {
            resourceParticalSpawner.SpawnPartical(workbenchZoneController.PlayerTransform.position, targetTransform.position, type);
        }
    }

    void UpdateCraftingAmountUI(int amount)
    {
        craftingResourceAmount.text = amount.ToString();
    }

    void UIInitialization()
    {
        requireResourcesPanel.SetResourceType(workbenchZoneController.RequireResource);
        requireResourcesPanel.SetResourceAmount(workbenchZoneController.RequireAmount);
        craftingResourceImage.sprite = workbenchZoneController.CraftingResource.resourceSprite;
    }
}
