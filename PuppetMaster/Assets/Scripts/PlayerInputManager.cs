using UnityEngine;

namespace Player.Input
{
    public class PlayerInputManager : MonoBehaviour
    {
        // An array of objects than can recieve input from this
        private IMoveInputReciever[] moveInputRecievers;

        private IActionInputReciever[] actionInputRecievers;

        private InputControls inputControls;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            // Retrieve all the input recievers on start
            // NOTE: This could become rather performance intensive if not kept under check
            moveInputRecievers = GetComponents<IMoveInputReciever>();
            actionInputRecievers = GetComponents<IActionInputReciever>();

            // Generate the input system
            inputControls = new InputControls();
            inputControls.Enable();

            // We add both to performed and canceled to ensure we recieve the released input.
            inputControls.InWorld.Move.performed += HandleMovement;
            inputControls.InWorld.Move.canceled += HandleMovement;

            // Setup the action button controls
            inputControls.InWorld.Fire1.performed += Fire1_performed;
            inputControls.InWorld.Fire2.performed += Fire2_performed;

            // Enable the input system
            SetPlayerControlEnabled(true);
        }

        private void Fire1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            foreach (var item in actionInputRecievers)
            {
                item.OnFire1();
            }
        }

        private void Fire2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            foreach (var item in actionInputRecievers)
            {
                item.OnFire2();
            }
        }

        private void HandleMovement(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            var move = obj.ReadValue<Vector2>();

            foreach (var item in moveInputRecievers)
            {
                item.HorizontalInput = move.x;
                item.VerticalInput = move.y;
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