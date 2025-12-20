using System.Collections;
using Cookie.InputHelper;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionButton : ActionDisplay
{
    [SerializeField]
    private float inDuration = 0.15f;

    [SerializeField]
    private float outDuration = 0.3f;

    [SerializeField]
    private float scale = 1.2f;

    private float _colourVel;
    private Coroutine _currentScale;
    private float _scaleProgress = 0;
    private InputActionReference Action => actions[0];

    protected override void OnEnable()
    {
        base.OnEnable();
        Action.action.Enable();
        Action.action.performed += OnActionPerformed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Action.action.performed -= OnActionPerformed;
    }

    private void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        if (_currentScale != null)
            StopCoroutine(_currentScale);
        _currentScale = StartCoroutine(AnimateScale());
    }

    // pain but sample shouldn't have dependencies
    private IEnumerator AnimateScale()
    {
        while (_scaleProgress < 1)
        {
            _scaleProgress += Time.deltaTime / inDuration;
            EaseScale(_scaleProgress);

            yield return null;
        }

        while (_scaleProgress > 0)
        {
            _scaleProgress -= Time.deltaTime / outDuration;
            EaseScale(_scaleProgress);

            yield return null;
        }

        _scaleProgress = 0;
    }

    private void EaseScale(float progress)
    {
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * scale, Ease(progress));
    }

    // Quadratic in out
    private static float Ease(float x) => x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
}
