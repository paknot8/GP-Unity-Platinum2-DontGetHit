using UnityEngine;

// This script is used to reference which ScriptableObject will be bound to a Button.
// Attach this script to a Button GameObject and drag the desired ScriptableObject into the Binding field.
public class AssignedBinding : MonoBehaviour
{
    public KeybindingScriptableObject binding; // The ScriptableObject to be bound to this Button.
}
