using Cookie.InputHelper;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionDisplay : MonoBehaviour
{
    [SerializeField] protected InputActionReference action;
    [SerializeField] protected TMP_Text text;


    protected virtual void OnEnable() {
        action.action.Enable();
        InputSystem.onActionChange += OnActionChange;
        UpdateText();
    }

    protected virtual void OnDisable() {
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change) {
        if (change != InputActionChange.BoundControlsChanged) return;
        var target = obj as InputAction;

        if (target != action.action) return;

        UpdateText();
    }

    private void UpdateText() {
        text.text = InputUtils.GetDisplay(action.action);
    }
}