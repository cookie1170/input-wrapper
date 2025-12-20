using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cookie.InputHelper
{
    [PublicAPI]
    public class ActionDisplay : MonoBehaviour
    {
        [SerializeField]
        protected InputActionReference[] actions;

        [SerializeField]
        protected TMP_Text text;

        protected string OriginalText = null;

        protected virtual void Reset()
        {
            text = GetComponentInChildren<TMP_Text>();
        }

        protected virtual void OnEnable()
        {
            InputSystem.onActionChange += OnActionChange;
            UpdateText();
        }

        protected virtual void OnDisable()
        {
            InputSystem.onActionChange -= OnActionChange;
        }

        private void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.BoundControlsChanged)
                return;

            UpdateText();
        }

        private void UpdateText()
        {
            if (string.IsNullOrEmpty(OriginalText))
                OriginalText = string.IsNullOrEmpty(text.text) ? "{0}" : text.text;

            text.text = string.Format(
                OriginalText,
                actions.Select(action => InputUtils.GetDisplay(action)).ToArray<object>()
            );
        }
    }
}
