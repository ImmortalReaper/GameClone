using System.Collections.Generic;
using UnityEngine;

public class ZoneView : MonoBehaviour
{
    [SerializeField] ZoneController zoneController;
    [SerializeField] ZoneAnimationController zoneAnimationController;

    [Header("Resource Particle System")]
    [SerializeField] ResourceParticalSpawner resourceParticalSpawner;
    [SerializeField] Transform targetTransform;

    [Header("UI Elements")]
    [SerializeField] GameObject requireResourcesUIPanelPrefab;
    [SerializeField] Transform requireResourcesUIPanelParent;

    [Header("Particle System")]
    [SerializeField] ParticleSystem depositResourceParticleSystem;

    List<RequireResourcesUIPanel> requireResourcesUIPanels;

    void Start()
    {
        requireResourcesUIPanels = new List<RequireResourcesUIPanel>();
        CreateRequiredResourcesPanels();
        ShowSavedResources();
    }

    void OnEnable()
    {
        zoneController.OnResourceDeposit += DepositResource;
        zoneController.OnZoneUnlock += zoneAnimationController.ShrinkAnimation;
        resourceParticalSpawner.OnParticleAnimationEnd += UpdateDepositResourceAmountUI;
    }

    void OnDestroy()
    {
        zoneController.OnResourceDeposit -= DepositResource;
        zoneController.OnZoneUnlock -= zoneAnimationController.ShrinkAnimation;
        resourceParticalSpawner.OnParticleAnimationEnd -= UpdateDepositResourceAmountUI;
    }

    public bool IsZoneUIComplete()
    {
        foreach (RequireResourcesUIPanel requiredResource in requireResourcesUIPanels)
        {
            if (!requiredResource.IsResourceComplete()) { return false; }
        }
        return true;
    }

    void ShowSavedResources()
    {
        for (int i = 0; i < requireResourcesUIPanels.Count; i++)
        {
            requireResourcesUIPanels[i].AddDepositResource(zoneController.RequiredResources[i].amount);
        }
    }

    void DepositResource(ResourceType type)
    {
        depositResourceParticleSystem.Emit(1);
        resourceParticalSpawner.SpawnPartical(zoneController.PlayerTransform.position, targetTransform.position, type);
    }

    void UpdateDepositResourceAmountUI(ResourceType type)
    {
        for(int i = 0; i < requireResourcesUIPanels.Count; i++)
        {
            if (requireResourcesUIPanels[i].resourceType.Equals(type))
            {
                requireResourcesUIPanels[i].AddDepositResource(1);
            }
        }
        if (IsZoneUIComplete()) { zoneController.UnlockZone(); }
    }

    void CreateRequiredResourcesPanels()
    {
        RequireResourcesUIPanel requireResourcesUIPanel = null;
        foreach (ZoneController.RequiredResource requiredResource in zoneController.RequiredResources)
        {
            requireResourcesUIPanel = Instantiate(requireResourcesUIPanelPrefab, requireResourcesUIPanelParent).GetComponent<RequireResourcesUIPanel>();
            requireResourcesUIPanel.SetRequireResourceAmount(requiredResource.requiredAmount);
            requireResourcesUIPanel.resourceType = requiredResource.resourceType;
            requireResourcesUIPanels.Add(requireResourcesUIPanel);
        }
    }
}
