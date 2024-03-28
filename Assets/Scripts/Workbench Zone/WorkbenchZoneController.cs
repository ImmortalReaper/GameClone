using BayatGames.SaveGameFree;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WorkbenchZoneController : MonoBehaviour, IDataPersistence
{
    [Header("Data Persistence")]
    [SerializeField] string objectId;

    [Header("Crafting Resource")]
    [SerializeField] ResourceType craftingResource;
    public ResourceType CraftingResource 
    {
        get { return craftingResource; }
        private set { craftingResource = value; }
    }
    [SerializeField] float craftingDurationInSeconds = 5f;

    [Header("Require Resource")]
    [SerializeField] ResourceType requireResource;
    public ResourceType RequireResource
    {
        get { return requireResource; }
        private set { requireResource = value; }
    }
    [SerializeField] int requireAmount;
    public int RequireAmount
    {
        get { return requireAmount; }
        private set { requireAmount = value; }
    }

    [Header("Zone")]
    [SerializeField] PlayerResources playerResources;
    [SerializeField] float depositeRateInSeconds = 0.1f;

    Timer depositTimer;

    public Timer CraftingTimer { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public int CraftingQueueCount { get; private set; } = 0;

    public Action<int> OnCraftingQueueCountChange;
    public Action<ResourceType, int> OnResourceDeposit;
    public Action OnWorkbenchFinishCrafting;
    public Action OnWorkbenchStartCrafting;

    [ContextMenu("Generate guid for id")]
    void GenerateGuid() => objectId = Guid.NewGuid().ToString();

    void Awake() => Load();
    void OnApplicationPause(bool pause) => Save();

    void Start()
    {
        CraftingTimer = new Timer(craftingDurationInSeconds);
        depositTimer = new Timer(depositeRateInSeconds);
        depositTimer.Start();
        if(CraftingQueueCount > 0)
        {
            ShowSavedCraftingItem();
        }
    }

    void FixedUpdate()
    {
        CraftingTimer.Tick(Time.deltaTime);
        depositTimer.Tick(Time.deltaTime);

        CraftItem();
    }

    void CraftItem()
    {
        if (CraftingTimer.IsFinish() && CraftingQueueCount > 0)
        {
            CraftingQueueCount--;
            playerResources.AddResource(craftingResource, 1);

            OnCraftingQueueCountChange?.Invoke(CraftingQueueCount);

            CraftingTimer.Reset();
            if (CraftingQueueCount == 0)
            {
                CraftingTimer.Stop();
                OnWorkbenchFinishCrafting?.Invoke();
            }
        }
    }

    void ShowSavedCraftingItem()
    {
        CraftingTimer.Start();
        OnWorkbenchStartCrafting?.Invoke();
        OnCraftingQueueCountChange?.Invoke(CraftingQueueCount);
    }

    public void DepositResources()
    {
        if (playerResources.Inventory.ContainsKey(requireResource) 
            && playerResources.GetResourceAmount(requireResource) >= requireAmount)
        {
            if (CraftingQueueCount == 0)
            {
                CraftingTimer.Start();
                OnWorkbenchStartCrafting?.Invoke();
            }
            CraftingQueueCount++;
            playerResources.RemoveResource(requireResource, requireAmount);

            OnCraftingQueueCountChange?.Invoke(CraftingQueueCount);
            OnResourceDeposit?.Invoke(requireResource, requireAmount);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerResources resources))
        {
            if (PlayerTransform == null) { PlayerTransform = resources.transform; }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerResources playerResources))
        {
            if (depositTimer.IsFinish())
            {
                DepositResources();
                depositTimer.Reset();
            }
        }
    }

    public void Save()
    {
        if (objectId.Length > 0)
        {
            SaveGame.Save(objectId, CraftingQueueCount);
        }
        else if (string.IsNullOrEmpty(objectId))
        {
            throw new ArgumentException("Object id cannot be empty.", nameof(objectId));
        }
    }

    public void Load()
    {
        if (SaveGame.Exists(objectId))
        {
            CraftingQueueCount = SaveGame.Load(objectId, CraftingQueueCount);
        }
    }
}
