using UnityEngine;
using PuppetMaster;

namespace Player.Input
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputControls inputControls;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            // Generate the input system
            inputControls = new InputControls();
            inputControls.Enable();

            // Setup the move and look controls
            // We add both to performed and canceled to ensure we recieve the released input.
            inputControls.InWorld.Move.performed += HandleMovement;
            inputControls.InWorld.Move.canceled += HandleMovement;

            inputControls.InWorld.Look.performed += HandleLook;
            inputControls.InWorld.Look.canceled += HandleLook;

            // Setup the action button controls
            inputControls.InWorld.Fire1.performed += Fire1_performed;
            inputControls.InWorld.Fire1.canceled += Fire1_canceled;

            inputControls.InWorld.Fire2.performed += Fire2_performed;
            inputControls.InWorld.Fire2.canceled += Fire2_canceled;

            // Enable the input system
            SetPlayerControlEnabled(true);
        }

        /// <summary>
        /// Sets the player controls to enabled of disabled.
        /// </summary>
        /// <param name="enabled"></param>
        public void SetPlayerControlEnabled(bool enabled)
        {
            if (enabled)
            {
                // Allow input to be sent
                inputControls.InWorld.Enable();
            }
            else
            {
                // Disallow input to be sent
                inputControls.InWorld.Disable();
            }
        }

        // Function managers handle sending input to the user controlled object.

        private void Fire1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            // Inform the player controlled object of the input action.
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire1_performed();
        }

        private void Fire1_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            // Inform the player controlled object of the input action.
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire1_canceled();
        }

        private void Fire2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            // Inform the player controlled object of the input action.
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire2_performed();
        }

        private void Fire2_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            // Inform the player controlled object of the input action.
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire2_canceled();
        }

        private void HandleMovement(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (CharacterInput.playerControlled != null)
            {
                // Read a vector2 from the input received
                var move = obj.ReadValue<Vector2>();

                // Send that vector2 to our player object
                CharacterInput.playerControlled.HandleMovement(move);
            }
        }

        private void HandleLook(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (CharacterInput.playerControlled != null)
            {
                // Read a vector2 from the input received
                var screenSpace = obj.ReadValue<Vector2>();

                var _transform = CharacterInput.playerControlled.transform;

                var value = Utility.Utilities.GetScreenSpaceOffsetFromObject(_transform, screenSpace);

                // Send that vector2 to our player object
                CharacterInput.playerControlled.HandleLook(screenSpace);
            }
        }
    }
}