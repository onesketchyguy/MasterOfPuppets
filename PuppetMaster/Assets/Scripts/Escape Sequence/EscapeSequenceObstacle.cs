using UnityEngine;

namespace PuppetMaster
{
    public class EscapeSequenceObstacle : MonoBehaviour
    {
        [SerializeField] private string playerTag = "Player";

        [SerializeField] private float spawnRange = 5f;
        [SerializeField] private float spawnHeight = 10;

        [SerializeField] private float setbackAmount = 1f;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private AudioClip onHitPlayerClip;

        [SerializeField] private AudioClipPlayer audioClipPlayer;

        private Transform m_transform;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.up * spawnHeight + Vector3.right * -spawnRange,
                Vector3.up * spawnHeight + Vector3.right * spawnRange);
        }

        private void OnEnable()
        {
            if (m_transform == null) m_transform = transform;

            OnCreation();
        }

        private void OnDisable()
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;

            m_transform.rotation = Quaternion.identity;
            m_transform.position = Vector3.zero;
        }

        private void Update()
        {
            if (TrySetRenderer() == false) return;

            if (_renderer.isVisible == false)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(playerTag))
            {
                var rigidBody = collision.gameObject.GetComponent<Rigidbody>();
                rigidBody.transform.position -= Vector3.up * setbackAmount;

                rigidBody.velocity = Vector3.up * -setbackAmount;

                audioClipPlayer.PlayClip(onHitPlayerClip);

                // Try to send an event to the reciever
                rigidBody.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);

                gameObject.SetActive(false);
            }
        }

        private bool TrySetRenderer()
        {
            if (_renderer == null)
            {
                _renderer = GetComponentInChildren<Renderer>();
            }

            return _renderer == null;
        }

        private void OnCreation()
        {
            m_transform.position = new Vector3(Random.Range(-spawnRange, spawnRange),
                spawnHeight);

            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }

        public void SetAudioClipPlayer(AudioClipPlayer audioClipPlayer)
        {
            this.audioClipPlayer = audioClipPlayer;
        }
    }
}