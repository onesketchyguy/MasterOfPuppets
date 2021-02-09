using UnityEngine;
using PuppetMaster;

namespace Player.Input
{
    public class PlayerInputManager : MonoBehaviour
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

            // We add both to performed and canceled to ensure we recieve the released input.
            inputControls.InWorld.Move.performed += HandleMovement;
            inputControls.InWorld.Move.canceled += HandleMovement;

            // Setup the action button controls
            inputControls.InWorld.Fire1.performed += Fire1_performed;
            inputControls.InWorld.Fire1.canceled += Fire1_canceled;

            inputControls.InWorld.Fire2.performed += Fire2_performed;
            inputControls.InWorld.Fire2.canceled += Fire2_canceled;

            // Enable the input system
            SetPlayerControlEnabled(true);
        }

        private void Fire1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire1_performed();
        }

        private void Fire1_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire1_canceled();
        }

        private void Fire2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire2_performed();
        }

        private void Fire2_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (CharacterInput.playerControlled != null)
                CharacterInput.playerControlled.Fire2_canceled();
        }

        private void HandleMovement(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (CharacterInput.playerControlled != null)
            {
                var move = obj.ReadValue<Vector2>();
                CharacterInput.playerControlled.HandleMovement(move);
            }
        }

        public void SetPlayerControlEnabled(bool enabled)
        {
            if (enabled)
            {
                inputControls.InWorld.Enable();
            }
            else
            {
                inputControls.InWorld.Disable();
            }
        }
    }
}