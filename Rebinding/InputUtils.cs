using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cookie.InputHelper
{
    [PublicAPI]
    public static class InputUtils
    {
        public static string GetDisplay(InputAction action) => GetDisplay(action, out _, out _, out _);

        public static string GetDisplay(
            InputAction action,
            out int bindingIndex,
            out string deviceLayoutName,
            out string controlPath
        ) {
            controlPath = null;
            deviceLayoutName = null;
            bindingIndex = -1;

            var bindings = action.bindings;

            if (bindings.Count <= 0) throw new ArgumentException("Action has no bindings!");

            for (int i = 0; i < bindings.Count; i++) {
                bindingIndex = i;

                string displayString = action.GetBindingDisplayString(
                    bindingIndex, out deviceLayoutName, out controlPath
                );

                if (!string.IsNullOrWhiteSpace(displayString)) return displayString;
            }

            Debug.LogWarning($"{action}'s display string is null or whitespace for all bindings");

            return controlPath ?? throw new Exception("Binding string is null or whitespace for all bindings!");
        }
    }
}