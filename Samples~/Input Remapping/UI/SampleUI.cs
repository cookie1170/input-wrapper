using Cookie.InputHelper;
using UnityEngine;
using UnityEngine.InputSystem;

public class SampleUI : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset asset;

    [SerializeField]
    private InputActionReference escapeAction;

    private void Awake()
    {
        escapeAction.action.Enable();
        escapeAction.action.performed += OnEscape;
        InputUtils.LoadBinds(asset);
    }

    private void OnDestroy()
    {
        escapeAction.action.performed -= OnEscape;
    }

    private void OnEscape(InputAction.CallbackContext ctx)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
