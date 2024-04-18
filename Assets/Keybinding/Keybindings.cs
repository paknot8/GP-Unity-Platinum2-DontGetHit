using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class Keybindings : MonoBehaviour
{
    #region Variables & References
        private PlayerInput input; // Player Input
        private RebindingOperation rebindingOperation; // Rebinding Operation

        // Scriptable Object
        private KeybindingScriptableObject currentBinding;
        private InputAction currentAction;

        // Showing all buttons currently on the screen
        [Header("Shows on Start")]
        public Button[] allButtons;
    #endregion

    #region Default Unity
        void Start()
        {
            // Get PlayerInput component
            if (!TryGetComponent<PlayerInput>(out input))
            {
                Debug.LogError("PlayerInput component not found.");
                return;
            }

            // Find all buttons
            allButtons = FindObjectsByType<Button>(FindObjectsSortMode.InstanceID);
            if (allButtons == null || allButtons.Length == 0)
            {
                Debug.LogError("No buttons found.");
                return;
            }

            // Load saved controls
            if (PlayerPrefs.HasKey("controls"))
            {
                input.actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("controls"));
            }

            // Update buttons text
            UpdateAllButtons();
        }
    #endregion

    #region Rebind Functions
        public void RebindButton(Button button)
        {
            SetBinding(button);

            // Disable the action before rebinding
            currentAction.Disable();

            // Start rebinding operation
            rebindingOperation = currentAction.PerformInteractiveRebinding()
                .WithTargetBinding(currentBinding.compositeNumber)
                .WithBindingGroup("Keyboard&Mouse")
                .WithControlsHavingToMatchPath("<Keyboard>")
                .WithControlsHavingToMatchPath("<Mouse>")
                // Remove canceling through Escape key
                // .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnPotentialMatch(operation => CheckBinding())
                .OnComplete(operation => { RebindComplete(button); })
                .OnCancel(operation => { RebindCancel(button); });

            // Set button text to indicate listening state
            button.GetComponentInChildren<TMP_Text>().text = "listening...";
            DisableAllButtons();
            rebindingOperation.Start();
        }


        private void CheckBinding()
        {
            string displayName = rebindingOperation.selectedControl.displayName;
            string shortDisplayName = rebindingOperation.selectedControl.shortDisplayName;

            // Iterate through all action maps in the PlayerInput
            foreach (var actionMap in input.actions.actionMaps)
            {
                // Iterate through all bindings in the action map
                foreach (var binding in actionMap.bindings)
                {
                    // Check if the binding matches the selected control's display name or short display name
                    if (binding.ToDisplayString().Equals(displayName) || binding.ToDisplayString().Equals(shortDisplayName))
                    {
                        // If the control is already bound to another action, cancel the rebinding operation
                        rebindingOperation.Cancel();
                        return;
                    }
                }
            }
            // If no conflicting bindings are found, complete the rebinding operation
            rebindingOperation.Complete();
        }

        private void UpdateButton(Button button)
        {
            button.GetComponentInChildren<TMP_Text>().text = button.GetComponent<AssignedBinding>().binding.GetBinding(input);
        }

        private void UpdateAllButtons()
        {
            foreach (Button button in allButtons)
            {
                if (!button.name.Equals("Back"))
                {
                    UpdateButton(button);
                }
            }
        }

        private void RebindComplete(Button button)
        {
            rebindingOperation.Dispose();
            UpdateButton(button);
            PlayerPrefs.SetString("controls", input.actions.SaveBindingOverridesAsJson());

            // Re-enable the action
            currentAction.Enable();

            EnableAllButtons();
        }

        private void RebindCancel(Button button)
        {
            rebindingOperation.Dispose();
            UpdateButton(button);

            // Re-enable the action
            currentAction.Enable();

            EnableAllButtons();
        }

        private void DisableAllButtons()
        {
            foreach (Button button in allButtons)
            {
                button.interactable = false;
            }
        }

        private void EnableAllButtons()
        {
            foreach (Button button in allButtons)
            {
                button.interactable = true;
            }
        }

        private void SetBinding(Button button)
        {
            currentBinding = button.GetComponent<AssignedBinding>().binding;
            SetAction(currentBinding.actionName);
        }
        private void SetAction(string actionName)
        {
            currentAction = input.actions.FindAction(actionName);
        }
    #endregion
}
