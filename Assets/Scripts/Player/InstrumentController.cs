using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InstrumentController : MonoBehaviour
{
    [SerializeField] AttackAnimationController attackAnimationController;
    [SerializeField] PlayerResources playerResources;
    [SerializeField] List<InstrumentGatherableResources> instruments;

    public BoxCollider InstrumentCollider { get; private set; }

    void Start()
    {
        InstrumentCollider = GetComponent<BoxCollider>();
    }

    void OnEnable()
    {
        attackAnimationController.OnCollisionExit += DisableInstrument;
    }

    void OnDestroy()
    {
        attackAnimationController.OnCollisionExit -= DisableInstrument;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourceController resourceController))
        {
            resourceController.TakeDamage(transform);
            playerResources.AddResource(resourceController.ResourceType, 1);
        }
    }

    public void EnableInstrument()
    {
        foreach (InstrumentGatherableResources instrument in instruments)
        {
            foreach (ResourceType resourceType in instrument.ResourceTypes)
            {
                ResourceType collectingResource = attackAnimationController.CollectingResource.ResourceType;
                if (resourceType == collectingResource)
                {
                    instrument.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public void DisableInstrument()
    {
        foreach (InstrumentGatherableResources instrument in instruments)
        {
            instrument.gameObject.SetActive(false);
        }
    }
}
