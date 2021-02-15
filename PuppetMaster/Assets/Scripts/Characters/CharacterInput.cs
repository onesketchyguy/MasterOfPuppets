using Player.Input;
using UnityEngine;

namespace PuppetMaster
{
    public class CharacterInput : MonoBehaviour
    {
        // An array of objects than can recieve input from this
        // NOTE: We could create a custom inspector to make these available to the designer.

        private IMoveInputReceiver[] moveInputReceivers;
        private IActionInputReceiver[] actionInputReceivers;
        private ILookInputReceiver[] lookInputReceivers;

        /// <summary>
        /// Returns the current player controlled object.
        /// </summary>
        public static CharacterInput playerControlled;

        /// <summary>
        /// Returns true if this is the static playerControlled object.
        /// </summary>
        public bool isPlayer
        {
            get
            {
                return playerControlled == this;
            }
        }

        [Tooltip("Sets this character to be the player on scene started.")]
        [SerializeField] private bool isPlayerOnStart = false;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            // Retrieve all the input recievers on start
            // NOTE: This could become rather performance intensive if not kept under check
            moveInputReceivers = GetComponents<IMoveInputReceiver>();
            actionInputReceivers = GetComponents<IActionInputReceiver>();
            lookInputReceivers = GetComponents<ILookInputReceiver>();

            // Set this as player
            if (isPlayerOnStart) playerControlled = this;
        }

        /// <summary>
        /// Send action 1 down input to all input receivers.
        /// </summary>
        public void Fire1_performed()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire1();
            }
        }

        /// <summary>
        /// Send action 1 up input to all input receivers.
        /// </summary>
        public void Fire1_canceled()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire1Up();
            }
        }

        /// <summary>
        /// Send action 2 down input to all input receivers.
        /// </summary>
        public void Fire2_performed()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire2();
            }
        }

        /// <summary>
        /// Send action 2 up input to all input receivers.
        /// </summary>
        public void Fire2_canceled()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire2Up();
            }
        }

        /// <summary>
        /// Send move input to all move input receivers.
        /// </summary>
        /// <param name="move"></param>
        public void HandleMovement(Vector2 move)
        {
            foreach (var item in moveInputReceivers)
            {
                item.HorizontalInput = move.x;
                item.VerticalInput = move.y;
            }
        }

        /// <summary>
        /// Send look input to all look input receivers.
        /// </summary>
        /// <param name="look"></param>
        public void HandleLook(Vector2 look)
        {
            foreach (var item in lookInputReceivers)
            {
                // Old, didn't feel like fingering this one out there, bud
                //item.lookDirection = new Vector3(look.x, 0, look.y);

                // This works... For now.
                item.lookDirection = Utility.Utilities.GetScreenSpaceOffsetFromObject(transform, look);
            }
        }
    }
}