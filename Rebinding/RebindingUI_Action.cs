using System;
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
        [Header("Action")] [SerializeField] private string label;

        [SerializeField] private InputActionReference action;

        [Header("References")] [SerializeField]
        private TMP_Text displayText;

        [SerializeField] private Button addBinding;
        [SerializeField] private Button resetToDefault;
        [SerializeField] private Transform bindingsContent;
        [SerializeField] private RebindingUI_Binding bindingPrefab;

        private readonly List<RebindingUI_Binding> _bindings = new();

        private void Start() {
            InitializeEvents();
            UpdateDisplay();
            RefreshBindingDisplays();
        }

        private void OnValidate() {
            UpdateDisplay();
        }

        private void AddBinding() {
            throw new NotImplementedException();
        }

        private void ResetToDefault() {
            action.action.RemoveAllBindingOverrides();
            RefreshBindingDisplays();
        }

        private void InitializeEvents() {
            addBinding.onClick.AddListener(AddBinding);
            resetToDefault.onClick.AddListener(ResetToDefault);
        }

        private void AddBindingDisplays() {
            for (int i = 0; i < action.action.bindings.Count; i++) {
                RebindingUI_Binding binding = Instantiate(bindingPrefab, bindingsContent);
                binding.Action = action;
                binding.BindingIndex = i;
                binding.OnRemoved.AddListener(() => _bindings.Remove(binding));
                binding.UpdateDisplay();
                _bindings.Add(binding);
            }
        }

        private void ClearBindingDisplays() {
            foreach (RebindingUI_Binding binding in _bindings) Destroy(binding.gameObject);

            _bindings.Clear();
        }

        private void RefreshBindingDisplays() {
            ClearBindingDisplays();
            AddBindingDisplays();
        }

        private void UpdateDisplay() {
            if (displayText) displayText.text = label;
        }
    }
}