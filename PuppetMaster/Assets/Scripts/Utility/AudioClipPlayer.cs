using UnityEngine;

namespace PuppetMaster
{
    public class AudioClipPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource source = null;

        [SerializeField] private AudioClip[] clips = null;

        private void OnValidate()
        {
            if (source == null)
            {
                source = GetComponent<AudioSource>();
            }
        }

        public void PlayClip(float volume = 1)
        {
            source.PlayOneShot(clips[Random.Range(0, clips.Length)], volume);
        }

        public void PlayClip(AudioClip clip, float volume = 1)
        {
            source.PlayOneShot(clip, volume);
        }
    }
}