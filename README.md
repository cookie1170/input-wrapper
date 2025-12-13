# input-helper

Generates static wrappers for strong-referencing input actions from Unity's Input system, as well as provides a system
for easily remapping and displaying inputs

# Input wrapper:

1. Open the project settings (Edit -> Project settings)
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

1. Add the `ActionDisplay` component to a game object
2. Assign the `TMP_Text` reference to the target text object
3. Assign references to all the actions, in the same order as they are in the text
4. Any {0}, {1}, (...) (the same as string.Format()) in the text are replaced with their corresponding action's key,
   matching the user's keyboard layout