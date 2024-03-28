using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceParticalSpawner : MonoBehaviour
{
    [SerializeField] List<ResourceType> resourcesParticals;
    [SerializeField] Vector3 targetOffset;

    public Action<ResourceType> OnParticleAnimationEnd;
    ResourceParticalAnimation resourceParticalAnimation;

    public void SpawnPartical(Vector3 spawnPosition, Vector3 targetPosition, ResourceType resourceType)
    {
        foreach(ResourceType resourceParticle in resourcesParticals)
        {
            if (resourceType.Equals(resourceParticle))
            {
                resourceParticalAnimation = Instantiate(resourceParticle.droppedResource, spawnPosition, Quaternion.identity).GetComponent<ResourceParticalAnimation>();
                resourceParticalAnimation.OnAnimationEnd += DepositCallback;
                resourceParticalAnimation.StartAnimation(spawnPosition, targetPosition, targetOffset, transform.forward);
            }
        }
    }

    private void DepositCallback(ResourceType type, ResourceParticalAnimation animation)
    {
        animation.OnAnimationEnd -= DepositCallback;
        OnParticleAnimationEnd?.Invoke(type);
    }
}
