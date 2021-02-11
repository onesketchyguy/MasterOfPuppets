using UnityEngine;

namespace PuppetMaster
{
    public class AudioClipPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource source = null;

        [SerializeField] private AudioClip[] clips;

        public void PlayClip(float volume = 1)
        {
            source.PlayOneShot(clips[Random.Range(0, clips.Length)], volume);
        }
    }
}