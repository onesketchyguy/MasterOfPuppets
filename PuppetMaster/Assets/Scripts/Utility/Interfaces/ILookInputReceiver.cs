using UnityEngine;

namespace PuppetMaster
{
    public interface ILookInputReceiver
    {
        public Vector3 lookDirection { get; set; }
    }
}