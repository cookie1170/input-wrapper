using UnityEngine;
using UnityEngine.InputSystem;

public class SampleUI : MonoBehaviour
{
    [SerializeField] private InputActionReference escapeAction;

    private void Awake() {
        escapeAction.action.Enable();
        escapeAction.action.performed += OnEscape;
    }

    private void OnDestroy() {
        escapeAction.action.performed -= OnEscape;
    }

    private void OnEscape(InputAction.CallbackContext ctx) {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}