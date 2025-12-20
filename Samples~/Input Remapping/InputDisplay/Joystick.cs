using Cookie.InputHelper;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : ActionDisplay
{
    [SerializeField]
    private Transform indicator;

    [SerializeField]
    private float smoothTime = 0.2f;
    private Vector2 _vel = Vector2.zero;

    private InputActionReference Action => actions[0];

    private void Update()
    {
        var dir = Action.action.ReadValue<Vector2>();
        indicator.localPosition = Vector2.SmoothDamp(
            indicator.localPosition,
            dir,
            ref _vel,
            smoothTime
        );
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Action.action.Enable();
    }
}
