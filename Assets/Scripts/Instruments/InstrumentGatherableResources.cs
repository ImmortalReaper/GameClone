using System.Collections.Generic;
using UnityEngine;

public class InstrumentGatherableResources : MonoBehaviour
{
    [SerializeField] List<ResourceType> resourceTypes = new List<ResourceType>();
    public List<ResourceType> ResourceTypes
    {
        get { return resourceTypes; }
        private set { resourceTypes = value; }
    }
}
