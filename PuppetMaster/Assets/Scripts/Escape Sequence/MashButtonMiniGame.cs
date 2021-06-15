using UnityEngine;
using Player.Input;

namespace PuppetMaster
{
    public class MashButtonMiniGame : MonoBehaviour, IActionInputReceiver
    {
        public string button = "Fire1";

        public UnityEngine.Events.UnityEvent onStart;

        /// <summary>
        /// FIXME: properly implement cancel
        /// </summary>
        public UnityEngine.Events.UnityEvent onCancel;

        public UnityEngine.Events.UnityEvent onButtonPress;
        public UnityEngine.Events.UnityEvent onFinish;
        public int buttonCount = 8;
        public int buttonCountRandomRange = 5;

        private bool gameStarted = false;
        private int buttonsRecieved = 0;
        private int targetButtons;

        public void OnFire1()
        {
            if (button == "Fire1")
            {
                OnRecievedButtonInput();
            }
        }

        public void OnFire1Up()
        {
            if (button == "Fire1Up")
            {
                OnRecievedButtonInput();
            }
        }

        public void OnFire2()
        {
            if (button == "Fire2")
            {
                OnRecievedButtonInput();
            }
        }

        public void OnFire2Up()
        {
            if (button == "Fire2Up")
            {
                OnRecievedButtonInput();
            }
        }

        private void Start()
        {
            Reset();

            targetButtons = Random.Range(buttonCount - buttonCountRandomRange,
                buttonCount + buttonCountRandomRange);
        }

        public void Reset()
        {
            gameStarted = false;
            buttonsRecieved = 0;
        }

        public void StartGame()
        {
            if (gameStarted == true) return;

            onStart?.Invoke();

            gameStarted = true;
        }

        private void OnRecievedButtonInput()
        {
            if (gameStarted == false) return;

            Debug.Log("PressedButton");

            buttonsRecieved++;

            onButtonPress?.Invoke();

            if (buttonsRecieved >= targetButtons)
            {
                onFinish?.Invoke();
                Reset();
            }
        }
    }
}