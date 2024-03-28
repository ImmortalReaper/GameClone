using System;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public Action<bool> OnBehaviorChange;

    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        OnBehaviorChange?.Invoke(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        OnBehaviorChange?.Invoke(false);
        base.OnPointerUp(eventData);
    }
}