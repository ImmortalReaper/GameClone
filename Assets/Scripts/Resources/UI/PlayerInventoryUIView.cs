using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUIView : MonoBehaviour
{
    [SerializeField] GameObject resourceUIPanelPrefab;
    [SerializeField] Transform resourceUIPanelParent;
    [SerializeField] PlayerResources playerResources;
    List<ResourcePanelUI> resourcesPanels;

    void Start()
    {
        resourcesPanels = new List<ResourcePanelUI>();
        ShowSavedResources();
    }

    void OnEnable()
    {
        playerResources.OnResourceAmountChange += UpdateResourcePanel;
    }

    void OnDestroy()
    {
        playerResources.OnResourceAmountChange -= UpdateResourcePanel;
    }

    void ShowSavedResources()
    {
        foreach (var resource in playerResources.Inventory)
        {
            UpdateResourcePanel(resource.Key);
        }
    }

    void CreateResourcePanel(ResourceType type)
    {
        ResourcePanelUI panel = Instantiate(resourceUIPanelPrefab, resourceUIPanelParent).GetComponent<ResourcePanelUI>();
        panel.SetResourceType(type);
        panel.SetResourceAmount(playerResources.GetResourceAmount(type));
        resourcesPanels.Add(panel);
    }

    void UpdateResourcePanel(ResourceType type)
    {
        if (!CheckIfResourcePanelUIExist(type)) { CreateResourcePanel(type); }

        foreach(ResourcePanelUI resourcesPanel in resourcesPanels)
        {
            if (resourcesPanel.ResourceType.Equals(type))
            {
                if (playerResources.GetResourceAmount(type) == 0) { resourcesPanel.gameObject.SetActive(false); }
                else { resourcesPanel.gameObject.SetActive(true); }
                resourcesPanel.SetResourceAmount(playerResources.GetResourceAmount(type));
            }
        }
    }

    bool CheckIfResourcePanelUIExist(ResourceType resourceType)
    {
        if (resourcesPanels.Count == 0) { return false; }
        foreach(ResourcePanelUI resourcesPanel in resourcesPanels)
        {
            if (resourceType.Equals(resourcesPanel.ResourceType)) { return true; }
        }
        return false;
    }
}
