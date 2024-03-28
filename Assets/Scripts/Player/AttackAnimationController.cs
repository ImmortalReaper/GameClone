using System;
using UnityEngine;

public class AttackAnimationController : MonoBehaviour
{
    [SerializeField] InstrumentController instrumentController;

    [Header("Animator Settings")]
    [SerializeField] Animator animator;
    [SerializeField] string animatorParameterName;

    [Header("Collider Settings")]
    [SerializeField] float triggerColliderRadius;
    [SerializeField] Vector3 triggerColliderOffset;

    bool previouslyInCollision;
    bool isInCollision;

    public Action OnCollisionEnter;
    public Action OnCollisionExit;
    public ResourceController CollectingResource { get; private set; }

    void FixedUpdate()
    {
        CheckForCollisions();
        IsEnterCollision();
        IsExitCollision();
    }

    void CheckForCollisions()
    {
        previouslyInCollision = isInCollision;
        isInCollision = false;
        CollectingResource = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + triggerColliderOffset, triggerColliderRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ResourceController resourceController))
            {
                isInCollision = true;
                CollectingResource = resourceController;
                break;
            }
        }

        animator.SetBool(animatorParameterName, isInCollision);
    }

    void IsEnterCollision()
    {
        if(!previouslyInCollision && isInCollision) OnCollisionEnter?.Invoke();
    }

    void IsExitCollision()
    {
        if (previouslyInCollision && !isInCollision) OnCollisionExit?.Invoke();
    }

    public void OnEnableHitBox() => instrumentController.InstrumentCollider.enabled = true;
    public void OnDisableHitBox() => instrumentController.InstrumentCollider.enabled = false;
    public void OnBeginAttack() => instrumentController.EnableInstrument();
    public void OnEndAttack() { }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward + triggerColliderOffset, triggerColliderRadius);
    }
}
