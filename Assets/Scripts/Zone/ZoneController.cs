using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ZoneAnimationController))]
public class ZoneController : MonoBehaviour, IDataPersistence
{
    [Serializable]
    public class RequiredResource
    {
        public ResourceType resourceType;
        public int requiredAmount;
        public int amount;
    }

    [Header("Data Persistence")]
    [SerializeField] string objectId;

    [Header("Zone Settings")]
    [SerializeField] float depositeRateInSeconds = 0.1f;
    [SerializeField] List<RequiredResource> requiredResources;
    public List<RequiredResource> RequiredResources 
    {
        get { return requiredResources; }
        private set { requiredResources = value; }
    }
    [SerializeField] List<GameObject> unlockingCells; 

    bool isZoneUnlocked = false;
    Timer depositeTimer;
    PlayerResources playerResources;

    public Action<ResourceType> OnResourceDeposit;
    public Action OnZoneUnlock;
    public Transform PlayerTransform { get; private set; } = null;

    [ContextMenu("Generate guid for id")]
    void GenerateGuid() => objectId = Guid.NewGuid().ToString();

    void Awake() => Load();
    void OnApplicationPause(bool pause) => Save();

    void Start()
    {
        depositeTimer = new Timer(depositeRateInSeconds);
        depositeTimer.Start();
        if (isZoneUnlocked) {
            UnlockZone(false);
            GetComponent<ZoneAnimationController>().RootTransform.gameObject.SetActive(false);
        }
    }

    void FixedUpdate() => depositeTimer.Tick(Time.deltaTime);

    public void DepositResources()
    {
        if (IsZoneNotRequireResources()) { return; }

        foreach (RequiredResource requiredResource in requiredResources)
        {
            if (playerResources.Inventory.ContainsKey(requiredResource.resourceType)
                && requiredResource.requiredAmount > requiredResource.amount
                && playerResources.GetResourceAmount(requiredResource.resourceType) > 0)
            {
                requiredResource.amount++;
                playerResources.RemoveResource(requiredResource.resourceType, 1);
                OnResourceDeposit?.Invoke(requiredResource.resourceType);
            }
        }
    }

    bool IsZoneNotRequireResources()
    {
        foreach (RequiredResource requiredResource in requiredResources)
        {
            if (requiredResource.requiredAmount > requiredResource.amount) { return false; }
        }
        return true;
    }

    public void UnlockZone(bool withAnimation = true)
    {
        foreach(GameObject unlockingCell in unlockingCells)
        {
            unlockingCell.gameObject.SetActive(true);
            if(unlockingCell.TryGetComponent(out GrowAnimation growAnimation) && withAnimation)
            {
                growAnimation.StartAnimation();
            }
        }
        isZoneUnlocked = true;
        OnZoneUnlock?.Invoke();
        Save();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerResources resources))
        {
            if (PlayerTransform == null) { PlayerTransform = resources.transform; }
            if (playerResources == null) { playerResources = resources; }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerResources playerResources))
        {
            if (depositeTimer.IsFinish())
            {
                DepositResources();
                depositeTimer.Reset();
            }
        }
    }

    public void Save()
    {
        if (objectId.Length > 0)
        {
            SaveData saveData = new SaveData(isZoneUnlocked);
            saveData.SetDepositResources(requiredResources);
            SaveGame.Save(objectId, saveData);
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
            SaveData saveData = new SaveData();
            saveData = SaveGame.Load(objectId, saveData);

            isZoneUnlocked = saveData.isZoneUnlocked;
            for (int i = 0; i < requiredResources.Count; i++)
            {
                requiredResources[i].amount = saveData.depositResources[i];
            }
        }
    }

    class SaveData
    {
        public bool isZoneUnlocked = false;
        public List<int> depositResources = new List<int>();

        public SaveData(bool isZoneUnlocked = false)
        {
            this.isZoneUnlocked = isZoneUnlocked;
        }

        public void SetDepositResources(List<RequiredResource> requiredResources)
        {
            foreach (RequiredResource requiredResource in requiredResources) 
            {
                depositResources.Add(requiredResource.amount);
            }
        }
    }
}


