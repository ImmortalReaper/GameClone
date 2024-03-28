using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    [Serializable]
    public struct ResourceState
    {
        public GameObject resourceCell;
        public int resourceThreshold;
    }

    [SerializeField] List<ResourceState> resourceStates;

    [SerializeField] ResourceParticalSpawner resourceParticalSpawner;

    [Header("Animations")]
    [SerializeField] GrowAnimation resetResourceAnimation;
    [SerializeField] CollectResourceUIAnimation collectResourceUIAnimation;

    [Header("Particle Systems")]
    [SerializeField] ParticleSystem onDamageParticleSystem;

    public void PlayDamageAnimation(Transform playerTransform, ResourceType resourceType)
    {
        collectResourceUIAnimation.StartAnimation();
        onDamageParticleSystem.Play();
        resourceParticalSpawner.SpawnPartical(transform.position, playerTransform.position, resourceType);
    }

    public void UpdateResourceState(int resourceAmount)
    {
        foreach (var state in resourceStates)
        {
            if (state.resourceCell.activeSelf && resourceAmount < state.resourceThreshold)
            {
                state.resourceCell.SetActive(false);
                break;
            }
        }
    }

    public void ResetResourceAnimation()
    {
        foreach (var state in resourceStates)
        {
            state.resourceCell.SetActive(true);
        }

        resetResourceAnimation.StartAnimation();
    }
}