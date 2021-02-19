using UnityEngine;

namespace PuppetMaster
{
    /// <summary>
    /// Interface for an interactable object.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Interacts with this object.
        /// </summary>
        /// <param name="sender"></param>
        public void Interact(GameObject sender);
    }
}