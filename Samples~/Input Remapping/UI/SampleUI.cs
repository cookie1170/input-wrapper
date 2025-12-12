using Cookie.InputHelper;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleUI : MonoBehaviour
{
    [SerializeField] private InputActionAsset asset;
    [SerializeField] private InputActionReference escapeAction;

    private void Awake() {
        escapeAction.action.Enable();
        escapeAction.action.performed += OnEscape;
        foreach (InputActionMap map in asset.actionMaps) InputUtils.LoadBinds(map);
    }

    private void OnDestroy() {
        escapeAction.action.performed -= OnEscape;
    }

    private void OnEscape(InputAction.CallbackContext ctx) {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}