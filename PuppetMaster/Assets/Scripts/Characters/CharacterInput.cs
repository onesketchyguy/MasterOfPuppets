using Player.Input;
using UnityEngine;

namespace PuppetMaster
{
    public class CharacterInput : MonoBehaviour
    {
        // An array of objects than can recieve input from this

        private IMoveInputReceiver[] moveInputReceivers;
        private IActionInputReceiver[] actionInputReceivers;
        private ILookInputReceiver[] lookInputReceivers;

        public static CharacterInput playerControlled;

        public bool isPlayer
        {
            get
            {
                return playerControlled == this;
            }
        }

        /// <summary>
        /// FIXME: Remove this shit
        /// </summary>
        public bool isPlayerOnStart = true;

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

            // DEBUG set this as player
            if (isPlayerOnStart)
            {
                playerControlled = this;
            }
        }

        public void Fire1_performed()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire1();
            }
        }

        public void Fire1_canceled()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire1Up();
            }
        }

        public void Fire2_performed()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire2();
            }
        }

        public void Fire2_canceled()
        {
            foreach (var item in actionInputReceivers)
            {
                item.OnFire2Up();
            }
        }

        public void HandleMovement(Vector2 move)
        {
            foreach (var item in moveInputReceivers)
            {
                item.HorizontalInput = move.x;
                item.VerticalInput = move.y;
            }
        }

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