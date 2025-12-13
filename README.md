# input-helper

Generates static wrappers for strong-referencing input actions from Unity's Input system, as well as provides a system
for easily remapping and displaying inputs

# Input wrapper:

1. Open the project settings (Edit → Project settings)
2. Set it up under the 'Input Wrapper' section
    1. Namespace - the namespace you want the wrappers to be under
    2. Folder - the folder you want the wrappers to be located in
3. Create an input actions asset with 'Generate C# Class' enabled
4. Add it to the list in the project settings
    1. Class Name - The name for the wrapper class
    2. Generated Name - The name for the C# class generated from the input actions asset
    3. Generated Namespace - The namespace for the C# class generated from the input actions asset
    4. Asset - The input actions asset itself

# Input remapping:

1. Add the `RebindingUI_Action` component to a game object
2. Assign all the references in the inspector
3. Assign the correct input action and label

# Input display:

1. Download [the icon pack by Xelu](https://thoseawesomeguys.com/prompts)
2. Batch rename the ones under "Keyboard & Mouse/{colour scheme}" to remove `_Key_Dark` or `_Key_Light` (Q_Key_Dark ->
   Q)
3. Import them into your project
4. Make a TextMeshPro sprite asset from them called `ButtonIcons` and place it under `Resources / Sprite Assets` (by
   default, see TextMeshPro project settings)
    1. Set all the icons' sprite mode to single
    2. Create a sprite atlas with "Allow Rotation" disabled
    3. Add all the icons to it
    4. Create a sprite asset from the atlas using [TMProSpriteAtlas](https://github.com/TBarendt/TMProSpriteAtlas) or
       manually (good luck)
5. Add the `ActionDisplay` component to a game object
6. Assign the `TMP_Text` reference to the target text object
7. Assign references to all the actions, in the same order as they are in the text
8. Any {0}, {1}, (...) (the same as string.Format()) in the text are replaced with their corresponding action's key,
   matching the user's keyboard layout