using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour, IDataPersistence
{
    Dictionary<ResourceType, int> inventory = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> Inventory
    {
        get { return inventory; }
        private set { inventory = value; }
    }

    [Header("Data Persistence")]
    [SerializeField] string inventoryIndetifier = "inventory";

    public Action<ResourceType> OnResourceAmountChange;

    void Awake() => Load();
    void OnApplicationPause(bool pause) => Save();

    public void AddResource(ResourceType resourceType, int amount)
    {
        if (inventory.ContainsKey(resourceType)) { inventory[resourceType] += amount; }
        else { inventory.Add(resourceType, amount); }
        OnResourceAmountChange?.Invoke(resourceType);
    }

    public bool RemoveResource(ResourceType resourceType, int amount)
    {
        if (inventory.ContainsKey(resourceType))
        {
            if (inventory[resourceType] - amount < 0) { return false; }
            inventory[resourceType] -= amount;
            OnResourceAmountChange?.Invoke(resourceType);
            return true;
        }
        return false;
    }

    public int GetResourceAmount(ResourceType resourceType) { return inventory[resourceType]; }

    public void Save()
    {
        Dictionary<string,int> resources = new Dictionary<string,int>();
        foreach (var resource in inventory) 
        {
            resources.Add(resource.Key.name, resource.Value);
        }
        SaveGame.Save(inventoryIndetifier, resources);
    }

    public void Load()
    {
        if (SaveGame.Exists(inventoryIndetifier)) 
        {
            Dictionary<string, int> resources = new Dictionary<string, int>();
            resources = SaveGame.Load(inventoryIndetifier, resources);

            ResourceType[] resourceTypes = Resources.LoadAll<ResourceType>("");

            foreach (var resource in resources)
            {
                ResourceType resourceType = Array.Find(resourceTypes, rt => rt.name == resource.Key);
                if (resourceType != null)
                {
                    inventory.Add(resourceType, resource.Value);
                }
            }
        }
    }
}
