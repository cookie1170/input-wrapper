using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Cookie.InputHelper
{
    // ReSharper disable once InconsistentNaming
    public class RebindingUI_Binding : MonoBehaviour
    {
        [SerializeField] private Button bind;
        [SerializeField] private Button remove;
        [SerializeField] private TMP_Text bindingLabel;

        [NonSerialized] public readonly UnityEvent OnRemoved = new();
        [NonSerialized] public InputActionReference Action;
        [NonSerialized] public int BindingIndex;

        private void Awake() {
            InitializeEvents();
        }

        private void InitializeEvents() {
            bind?.onClick.AddListener(PerformRebind);
            remove?.onClick.AddListener(RemoveBinding);
        }

        private void RemoveBinding() {
            OnRemoved?.Invoke();
            Destroy(gameObject);
        }

        private void PerformRebind() {
            throw new NotImplementedException();
        }

        public void UpdateDisplay() {
            bindingLabel.text = Action.action.GetBindingDisplayString(BindingIndex);
        }
    }
}