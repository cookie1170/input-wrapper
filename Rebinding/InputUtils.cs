using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace Cookie.InputHelper
{
    [PublicAPI]
    public static class InputUtils
    {
        private const string IconsAsset = "ButtonIcons";

        // there's gotta be a better way to do this than /[1-9]|10|11|12/ right?
        private static readonly Regex Chars = new("^[A-Z1-9]$|^F([1-9]|10|11|12)$");

        /// <summary>
        ///     A lookup table for icon names
        /// </summary>
        /// <remarks>
        ///     They're made to fit the input prompts by Xelu (https://thoseawesomeguys.com/prompts/)
        ///     with suffixes for keyboard (like _Key_Dark, Q_Key_Dark -> Q) removed
        /// </remarks>
        private static readonly Dictionary<string, string> IconsLookup = new()
        {
            { "leftShift", "Shift" },
            { "rightShift", "Shift" },
            { "leftCtrl", "Ctrl" },
            { "rightCtrl", "Ctrl" },
            { "leftAlt", "Alt" },
            { "rightAlt", "Alt" },
            { "leftMeta", "Win" },
            { "leftBracket", "Bracket_Left" },
            { "rightBracket", "Bracket_Right" },
            { "semicolon", "Semicolon" },
            { "quote", "Quote" },
            { "backslash", "Slash" },
            { "comma", "Mark_Left" }, // same key
            { "period", "Mark_Right" }, // same key
            { "slash", "Question" }, // "Slash" is a backslash
            { "minus", "Minus" },
            { "equals", "Plus" }, // same key
            { "backspace", "Backspace" },
            { "enter", "Enter" },
            { "escape", "Esc" },
            { "backquote", "Tilda" },
            { "tab", "Tab" },
            { "capsLock", "Caps_Lock" },
            { "leftArrow", "Arrow_Left" },
            { "rightArrow", "Arrow_Right" },
            { "downArrow", "Arrow_Down" },
            { "upArrow", "Arrow_Up" },
            { "delete", "Del" },
            { "insert", "Insert" },
            { "home", "Home" },
            { "end", "End" },
            { "pageDown", "Page_Down" },
            { "pageUp", "Page_Up" },
            { "printScreen", "Print_Screen" },
            { "numLock", "Num_Lock" },
            { "numpadDivide", "Slash" }, // closest i could find, lmk if i should use something else
            { "numpadMultiply", "Asterisk" },
            { "numpadMinus", "Minus" },
            { "numpadPlus", "Plus_Tall" },
            { "numpadEnter", "Enter_Tall" },
            { "numpadPeriod", "Del" }, // same key
            { "numpad0", "0" },
            { "numpad1", "1" },
            { "numpad2", "2" },
            { "numpad3", "3" },
            { "numpad4", "4" },
            { "numpad5", "5" },
            { "numpad6", "6" },
            { "numpad7", "7" },
            { "numpad8", "8" },
            { "numpad9", "9" },
            { "space", "Space" },
            { "leftButton", "Mouse_Left" },
            { "rightButton", "Mouse_Right" },
            { "middleButton", "Mouse_Middle" },
        };

        /// <inheritdoc cref="GetDisplay(InputAction, out int, out string, out string)" />
        public static string GetDisplay(InputAction action) =>
            GetDisplay(action, out _, out _, out _);

        /// <summary>
        ///     Gets a display string for the action using icons from input prompts by Xelu (https://thoseawesomeguys.com/prompts/)
        ///     <br />
        ///     with any suffixes for keyboard (like _Key_Dark, Q_Key_Dark.pn should be just Q.png) removed, from a sprite asset
        ///     called 'ButtonIcons' for TextMeshPro
        ///     <br />
        ///     which should be under the path in your TextMeshPro project settings (by default "Resources/Sprite Assets")
        /// </summary>
        /// <param name="action">The input action to get a display string for</param>
        /// <param name="bindingIndex">The binding index of the final string</param>
        /// <param name="deviceLayoutName">
        ///     The name of the InputControlLayout used for the device in the given binding, if
        ///     applicable
        /// </param>
        /// <param name="controlPath">The path to the control on the device</param>
        /// <returns>The display string</returns>
        public static string GetDisplay(
            InputAction action,
            out int bindingIndex,
            out string deviceLayoutName,
            out string controlPath
        )
        {
            controlPath = null;
            deviceLayoutName = null;
            bindingIndex = -1;

            var bindings = action.bindings;

            for (int i = 0; i < bindings.Count; i++)
            {
                bindingIndex = i;

                string displayString = GetBindingDisplayString(
                    action,
                    bindingIndex,
                    out deviceLayoutName,
                    out controlPath
                );

                if (!string.IsNullOrEmpty(displayString))
                    return displayString;
            }

            return "Unbound";
        }

        /// <inheritdoc cref="GetDisplay(InputAction, out int, out string, out string)" />
        public static string GetBindingDisplayString(
            InputAction action,
            int bindingIndex,
            out string deviceLayoutName,
            out string controlPath
        )
        {
            if (action.bindings[bindingIndex].isComposite)
            {
                List<string> strings = new();

                deviceLayoutName = null;
                controlPath = null;

                for (
                    int i = bindingIndex + 1;
                    i < action.bindings.Count && action.bindings[i].isPartOfComposite;
                    i++
                )
                    strings.Add(
                        GetBindingDisplayString(action, i, out deviceLayoutName, out controlPath)
                    );

                return string.Join(' ', strings);
            }

            string displayString = action.GetBindingDisplayString(
                bindingIndex,
                out deviceLayoutName,
                out controlPath
            );
            string iconText;

            // This will match for any latin character and digit so if you have a layout like AZERTY, it'll still show the correct keys
            if (Chars.IsMatch(displayString))
            {
                iconText = displayString;

                goto end;
            }

            // Fallback: will work for characters and numbers in non-latin layouts, as well as the F keys
            // (so something like 'Я' in russian will show a 'Z' icon, since its controlPath is still 'z')
            string upperPath = controlPath.ToUpper();
            if (Chars.IsMatch(upperPath))
            {
                iconText = upperPath;

                goto end;
            }

            iconText = IconsLookup.GetValueOrDefault(controlPath, "Question"); // Using a question mark as unknown

            end:

            // offset because they're aligned to the top (?) for some reason
            return $"<voffset=-0.2em><sprite=\"{IconsAsset}\" name=\"{iconText}\"></voffset>";
        }

        // Credit to https://discussions.unity.com/t/list-of-all-inputcontrolpath/909946/5
        public static void DumpControlPaths()
        {
            var layouts = InputSystem
                .ListLayouts()
                .Select(InputSystem.LoadLayout)
                .Where(l => l.isGenericTypeOfDevice);
            foreach (InputControlLayout layout in layouts)
            {
                InputDevice device = InputSystem.AddDevice(layout.name);
                foreach (InputControl control in device.allControls)
                {
                    string relativePath = control.path.Substring(device.path.Length);
                    Debug.Log($"<{layout.name}>{relativePath}");
                }

                InputSystem.RemoveDevice(device);
            }
        }

        /// <summary>
        ///     Saves binding overrides to PlayerPrefs for the specified map
        /// </summary>
        /// <param name="map">The map to save for</param>
        /// <remarks>
        ///     The saved key is "Binds__{mapName}{assetName (if any)}"
        /// </remarks>
        /// <seealso cref="LoadBinds(InputActionMap)" />
        public static void SaveBinds(InputActionMap map)
        {
            string binds = map.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(GetMapKey(map), binds);
        }

        /// <summary>
        ///     Saves binding overrides for all InputActionMaps in the asset
        /// </summary>
        /// <param name="asset">The asset to save</param>
        /// <seealso cref="SaveBinds(InputActionMap)" />
        public static void SaveBinds(InputActionAsset asset)
        {
            foreach (InputActionMap map in asset.actionMaps)
                SaveBinds(map);
        }

        /// <summary>
        ///     Loads binding overrides from PlayerPrefs (if any) for the specified map
        /// </summary>
        /// <param name="map">The map to load for</param>
        /// <remarks>
        ///     The key is "Binds__{mapName}{assetName (if any)}"
        /// </remarks>
        /// <seealso cref="SaveBinds(InputActionMap)" />
        public static void LoadBinds(InputActionMap map)
        {
            string binds = PlayerPrefs.GetString(GetMapKey(map), null);

            if (binds == null)
                return;

            map.LoadBindingOverridesFromJson(binds);
        }

        /// <summary>
        ///     Loads binding overrides for all input action maps in the asset
        /// </summary>
        /// <param name="asset">The asset to load</param>
        /// <seealso cref="LoadBinds(InputActionMap)" />
        public static void LoadBinds(InputActionAsset asset)
        {
            foreach (InputActionMap map in asset.actionMaps)
                LoadBinds(map);
        }

        private static string GetMapKey(InputActionMap map) =>
            $"Binds__{map.name}{(map.asset ? map.asset.name : "")}";
    }
}
