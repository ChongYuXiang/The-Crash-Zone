using System.Text;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public static class RebindUtils
{
    public static string GetReadableBindingName(InputAction action, int bindingIndex)
    {
        var binding = action.bindings[bindingIndex];

        // Composite display (e.g., "W / S")
        if (binding.isComposite)
        {
            StringBuilder compositeDisplay = new StringBuilder();
            for (int i = bindingIndex + 1; i < action.bindings.Count; i++)
            {
                var partBinding = action.bindings[i];
                if (!partBinding.isPartOfComposite)
                    break;

                string partDisplay = GetSingleBindingName(partBinding);
                if (!string.IsNullOrEmpty(partDisplay))
                {
                    if (compositeDisplay.Length > 0)
                        compositeDisplay.Append(" / ");
                    compositeDisplay.Append(partDisplay);
                }
            }

            return compositeDisplay.ToString();
        }

        // Normal binding
        return GetSingleBindingName(binding);
    }

    private static string GetSingleBindingName(InputBinding binding)
    {
        if (string.IsNullOrEmpty(binding.effectivePath))
            return binding.ToDisplayString();

        InputControl control = InputSystem.FindControl(binding.effectivePath);
        if (control is KeyControl keyControl)
        {
            switch (keyControl.keyCode)
            {
                case Key.LeftShift: return "Left Shift";
                case Key.RightShift: return "Right Shift";
                case Key.LeftCtrl: return "Left Ctrl";
                case Key.RightCtrl: return "Right Ctrl";
                case Key.LeftAlt: return "Left Alt";
                case Key.RightAlt: return "Right Alt";
                default:
                    return InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            }
        }

        return InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}