using Player.Input;
using UnityEngine;

namespace PuppetMaster
{
    public class CharacterInput : MonoBehaviour
    {
        // An array of objects than can recieve input from this
        private IMoveInputReciever[] moveInputRecievers;

        private IActionInputReciever[] actionInputRecievers;

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
            moveInputRecievers = GetComponents<IMoveInputReciever>();
            actionInputRecievers = GetComponents<IActionInputReciever>();

            // DEBUG set this as player
            if (isPlayerOnStart)
            {
                playerControlled = this;
            }
        }

        public void Fire1_performed()
        {
            foreach (var item in actionInputRecievers)
            {
                item.OnFire1();
            }
        }

        public void Fire1_canceled()
        {
            foreach (var item in actionInputRecievers)
            {
                item.OnFire1Up();
            }
        }

        public void Fire2_performed()
        {
            foreach (var item in actionInputRecievers)
            {
                item.OnFire2();
            }
        }

        public void Fire2_canceled()
        {
            foreach (var item in actionInputRecievers)
            {
                item.OnFire2Up();
            }
        }

        public void HandleMovement(Vector2 move)
        {
            foreach (var item in moveInputRecievers)
            {
                item.HorizontalInput = move.x;
                item.VerticalInput = move.y;
            }
        }
    }
}