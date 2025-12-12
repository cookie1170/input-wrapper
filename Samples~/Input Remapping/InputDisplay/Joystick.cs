using UnityEngine;

public class Joystick : ActionDisplay
{
    [SerializeField] private Transform indicator;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector2 _vel = Vector2.zero;

    private void Update() {
        var dir = action.action.ReadValue<Vector2>();
        indicator.localPosition = Vector2.SmoothDamp(indicator.localPosition, dir, ref _vel, smoothTime);
    }
}