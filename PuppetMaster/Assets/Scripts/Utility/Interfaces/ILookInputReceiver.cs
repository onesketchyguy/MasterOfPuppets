using UnityEngine;

namespace Player.Input
{
    public interface ILookInputReceiver
    {
        public Vector3 lookDirection { get; set; }
    }
}