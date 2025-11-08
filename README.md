# input-wrapper

Generates static wrappers for strong-referencing input actions from Unity's Input system.

# Usage

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