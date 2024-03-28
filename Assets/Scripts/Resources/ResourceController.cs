using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class ResourceController : MonoBehaviour
{
    [SerializeField] ResourceView resourceView;

    [Header("Resource Data")]
    [SerializeField] ResourceType resourceType;
    public ResourceType ResourceType
    {
        get { return resourceType; }
        private set { resourceType = value; }
    }

    [SerializeField] int maxResourceAmount;
    [SerializeField] float resetResourceDuration;
    int resourceAmount;

    CapsuleCollider resourceCollider;
    public Action OnResourceDamage;
    public Action OnResourceReset;

    void Start()
    {
        resourceAmount = maxResourceAmount;
        resourceCollider = GetComponent<CapsuleCollider>();
    }

    public void TakeDamage(Transform playerTransform = null)
    {
        resourceAmount--;
        UpdateState();
        resourceView.PlayDamageAnimation(playerTransform, ResourceType);
        OnResourceDamage?.Invoke();
    }

    void UpdateState()
    {
        resourceView.UpdateResourceState(resourceAmount);

        if (resourceAmount <= 0)
        {
            resourceCollider.enabled = false;
            StartCoroutine(ResetResource());
        }
    }

    void SetDefaultState()
    {
        resourceAmount = maxResourceAmount;
        OnResourceReset?.Invoke();
        resourceCollider.enabled = true;
    }

    IEnumerator ResetResource()
    {
        yield return new WaitForSeconds(resetResourceDuration);
        SetDefaultState();
        resourceView.ResetResourceAnimation();
    }
}