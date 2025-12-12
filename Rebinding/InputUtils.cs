using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cookie.InputHelper
{
    [PublicAPI]
    public static class InputUtils
    {
        public static event Action<InputActionReference> OnActionRebound;

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

            for (int i = 0; i < bindings.Count; i++) {
                bindingIndex = i;

                string displayString = GetBindingDisplayString(
                    action, bindingIndex, out deviceLayoutName, out controlPath
                );

                if (!string.IsNullOrEmpty(displayString)) return displayString;
            }

            return "Unbound";
        }

        //TODO: icons?
        public static string GetBindingDisplayString(
            InputAction action,
            int bindingIndex,
            out string deviceLayoutName,
            out string controlPath
        ) {
            string displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath);

            // for SOME reason some keys (escape, backspace, enter) aren't mapped to DISPLAY strings but to unicode characters because why not at this point
            displayString = displayString.Replace("\u001b", "Escape");
            displayString = displayString.Replace("\b", "Backspace");

            return displayString;
        }

        public static void SaveBinds(InputActionMap map) {
            string binds = map.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(GetMapKey(map), binds);
        }

        public static void LoadBinds(InputActionMap map) {
            string binds = PlayerPrefs.GetString(GetMapKey(map), null);

            if (binds == null) return;

            map.LoadBindingOverridesFromJson(binds);
        }

        private static string GetMapKey(InputActionMap map) => $"Binds__{map.name}{(map.asset ? map.asset.name : "")}";

        public static void InvokeOnActionRebound(InputActionReference action) {
            OnActionRebound?.Invoke(action);
        }
    }
}