using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Cookie.InputHelper
{
    // ReSharper disable once InconsistentNaming
    public class RebindingUI_Action : MonoBehaviour
    {
        [Header("Action")]
        [SerializeField]
        private string label;

        [SerializeField]
        private InputActionReference action;

        [SerializeField]
        private int bindingIndex = 0;

        [SerializeField]
        private float timeout = 2;

        [Header("References")]
        [SerializeField]
        private TMP_Text displayText;

        [SerializeField]
        private TMP_Text bindingLabel;

        [SerializeField]
        private Button bind;

        [SerializeField]
        private Button resetToDefault;

        [SerializeField]
        private Button resetAll;

        [SerializeField]
        private GameObject rebindingScreen = null;

        [SerializeField]
        private TMP_Text rebindingScreenText = null;

        private List<(string, string)> _compositeParts;

        private void Start()
        {
            Initialize();
            UpdateDisplay();
        }

        private void OnDestroy()
        {
            bind?.onClick.RemoveListener(StartRebind);
            resetAll?.onClick.RemoveListener(ResetToDefault);
            resetToDefault?.onClick.RemoveListener(ResetToDefault);
        }

        private void OnValidate()
        {
            UpdateDisplay();
        }

        private void ResetToDefault()
        {
            action.action.RemoveAllBindingOverrides();
            UpdateDisplay();
            InputUtils.SaveBinds(action.action.actionMap);
        }

        private void StartRebind()
        {
            using var enumerator = GetRebindingIndices().GetEnumerator();
            PerformRebind(enumerator);
        }

        private void PerformRebind(IEnumerator<int> indices)
        {
            int index = indices.MoveNext() ? indices.Current : -1;

            if (index == -1)
                return;
            bool wasEnabled = action.action.enabled;

            // Need to disable it because otherwise stuff breaks
            action.action.Disable();
            InputBinding binding = action.action.bindings[index];
            if (rebindingScreenText)
                rebindingScreenText.text = binding.isPartOfComposite
                    ? $"Waiting for {binding.name} input..."
                    : "Waiting for input...";

            action
                .action.PerformInteractiveRebinding(index)
                .WithTimeout(timeout)
                .WithMatchingEventsBeingSuppressed()
                .WithActionEventNotificationsBeingSuppressed()
                .WithCancelingThrough("<keyboard>/escape")
                .OnCancel(Cleanup)
                .OnComplete(op =>
                {
                    Cleanup(op);

                    if (!binding.isPartOfComposite)
                        return;

                    PerformRebind(indices);
                })
                .Start();

            rebindingScreen?.SetActive(true);

            return;

            void Cleanup(InputActionRebindingExtensions.RebindingOperation op)
            {
                op?.Dispose();
                UpdateDisplay();
                rebindingScreen?.SetActive(false);
                InputUtils.SaveBinds(action.action.actionMap);
                if (wasEnabled)
                    action.action.Enable();
            }
        }

        private IEnumerable<int> GetRebindingIndices()
        {
            if (!action.action.bindings[bindingIndex].isComposite)
            {
                yield return bindingIndex;

                yield break;
            }

            for (
                int i = bindingIndex + 1;
                i < action.action.bindings.Count && action.action.bindings[i].isPartOfComposite;
                i++
            )
                yield return i;
        }

        private void Initialize()
        {
            bind?.onClick.AddListener(StartRebind);
            resetAll?.onClick.AddListener(ResetToDefault);
            resetToDefault?.onClick.AddListener(ResetToDefault);
        }

        private void UpdateDisplay()
        {
            if (displayText)
                displayText.text = label;

            if (action == null || action.action == null)
                return;

            string displayString = InputUtils.GetBindingDisplayString(
                action.action,
                bindingIndex,
                out _,
                out _
            );
            if (bindingLabel)
                bindingLabel.text = displayString;
        }
    }
}
