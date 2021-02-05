using UnityEngine;
using UnityEngine.InputSystem;

namespace Convienience
{
    /// <summary>
    /// A tool bag to help speed things up a bit.
    /// </summary>
    public static class Utilities
    {
        private static Camera mainCamera
        {
            get
            {
                if (_cam == null) _cam = Camera.main;
                return _cam;
            }
        }

        private static Camera _cam;

        private static Vector2 mousePos => mouse.position.ReadValue();

        private static Mouse mouse
        {
            get
            {
                if (_mouse == null) _mouse = Mouse.current;

                return _mouse;
            }
        }

        private static Mouse _mouse;

        public static T[] AddToArray<T>(T[] array, T item)
        {
            if (array == null)
            {
                return new T[] { item };
            }
            else
            {
                var old = array;
                array = new T[old.Length + 1];

                for (int i = 0; i < old.Length; i++)
                {
                    array[i] = old[i];
                }

                array[old.Length] = item;

                return array;
            }
        }

        /// <summary>
        /// Get the screen offset for an object
        /// </summary>
        /// <param name="_transform"></param>
        /// <returns></returns>
        public static Vector3 GetMouseOffsetFromObject(Transform _transform, float MAX_MOUSE_OFFSET = 1)
        {
            // Get the direction of the attack
            var screenSpace = mousePos;

            screenSpace.x = mouse.position.x.ReadValue();
            screenSpace.y = mouse.position.y.ReadValue();

            // Scale the space
            var objectPosition = (Vector2)mainCamera.WorldToScreenPoint(_transform.position);

            screenSpace -= objectPosition;
            screenSpace *= Time.deltaTime;

            var direction = Vector3.zero;

            // Set the X direction
            if (screenSpace.x > MAX_MOUSE_OFFSET || screenSpace.x < -MAX_MOUSE_OFFSET)
                direction.x = Mathf.Clamp(screenSpace.x, -MAX_MOUSE_OFFSET, MAX_MOUSE_OFFSET);
            else direction.x = screenSpace.x;

            // Set the Z direction
            if (screenSpace.y > MAX_MOUSE_OFFSET || screenSpace.y < -MAX_MOUSE_OFFSET)
                direction.z = Mathf.Clamp(screenSpace.y, -MAX_MOUSE_OFFSET, MAX_MOUSE_OFFSET);
            else direction.z = screenSpace.y;

            return direction;
        }

        /// <summary>
        /// Returns the mouse's position in world space.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMousePosition()
        {
            Vector3 moPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));

            return moPos;
        }

        /// <summary>
        /// Converts a Vector3 to a viewport position.
        /// </summary>
        /// <param name="worldpoint"></param>
        /// <returns></returns>
        public static Vector3 ToViewportSpace(Vector3 worldpoint)
        {
            return mainCamera.WorldToViewportPoint(worldpoint);
        }

        /// <summary>
        /// Converts a Vector3 to screen space.
        /// </summary>
        /// <param name="worldpoint"></param>
        /// <returns></returns>
        public static Vector3 ToScreenSpace(Vector3 worldpoint)
        {
            return mainCamera.WorldToScreenPoint(worldpoint);
        }

        /// <summary>
        /// Returns the mouse position locked to the grid.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GridLockedMousePosition()
        {
            Vector2 moPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

            moPos.x = (int)moPos.x;
            moPos.y = (int)moPos.y;

            return moPos;
        }

        /// <summary>
        /// Returns a random RGB value.
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);

            return new Color(r, g, b, 1);
        }

        /// <summary>
        /// Returns the top right corner of the screen.
        /// </summary>
        public static Vector3 ScreenMax { get { return mainCamera.ViewportToWorldPoint(new Vector3(1, 1)); } }

        /// <summary>
        /// Returns the absolute middle of the screen.
        /// </summary>
        public static Vector3 ScreenMid { get { return mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)); } }

        /// <summary>
        /// Returns the bottom left corner of the screen.
        /// </summary>
        public static Vector3 ScreenMin { get { return mainCamera.ViewportToWorldPoint(new Vector3(0, 0)); } }

        /// <summary>
        /// Returns true if the given position is not within the bounds of the screen.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="ScreenPadding"></param>
        /// <returns></returns>
        public static bool OffScreen(Vector3 position, float ScreenPadding = 1)
        {
            float minY = ScreenMin.y - ScreenPadding;
            float maxY = ScreenMax.y + ScreenPadding;

            float minX = ScreenMin.x - ScreenPadding;
            float maxX = ScreenMax.x + ScreenPadding;

            return (position.y > maxY) || (position.y < minY) || (position.x > maxX) || (position.x < minX);
        }

        /// <summary>
        /// A delegate for a value being modified
        /// </summary>
        /// <param name="amount"></param>
        public delegate void ValueModified(float amount);
    }
}