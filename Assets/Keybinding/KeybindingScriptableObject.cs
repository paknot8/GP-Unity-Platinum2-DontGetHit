using UnityEngine;
using UnityEngine.InputSystem;

// This script defines a ScriptableObject used for creating keybindings.
// It allows defining the action name and optionally a composite binding.
[CreateAssetMenu(fileName = "New Keybinding", menuName = "ScriptableObjects/Keybinding")]
public class KeybindingScriptableObject : ScriptableObject
{
    // The name of the action associated with this keybinding.
    public string actionName;

    // Indicates whether this binding is a composite binding.
    public bool isComposite = false;

    // If this binding is a composite, specifies the name of the composite part.
    public string compositePartName;

    // If this binding is a composite, specifies the index of the composite part.
    public int compositeNumber;

    // Retrieves the binding display string for this keybinding.
    public string GetBinding(PlayerInput playerInput)
    {
        if (!isComposite)
        {
            // If it's not a composite binding, return the display string for the action.
            return playerInput.actions.FindAction(actionName).GetBindingDisplayString();
        }
        else
        {
            // If it's a composite binding, return the display string for the specified composite part.
            var action = playerInput.actions.FindAction(actionName);
            return action.GetBindingDisplayString(compositeNumber);
        }
    }
    
    // CompositeNumber meaning:
    // 0 = no composite, so normal key, 1 = w/s/a/d 2 = w 3 = s 4 = a, 5 = d
}
