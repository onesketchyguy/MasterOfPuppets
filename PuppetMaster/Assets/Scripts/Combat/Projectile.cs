using UnityEngine;

namespace PuppetMaster
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float shotForce = 100;
        [SerializeField] private Rigidbody rigidBody;

        [SerializeField] private TrailRenderer trailRenderer;

        [SerializeField] private float lifeTime = 3;

        private int damageOnHit = 0;

        private Transform sender;

        public void SetSender(Transform sender)
        {
            this.sender = sender;
        }

        public void SetDamage(int damage)
        {
            damageOnHit = damage;
        }

        private float timeAlive = 0;

        private void OnEnable()
        {
            // Go flying in a direction
            rigidBody.AddForce(transform.forward * shotForce, ForceMode.Impulse);

            timeAlive = 0;
        }

        private void Update()
        {
            if (timeAlive > lifeTime)
            {
                gameObject.SetActive(false);
            }
            else timeAlive += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var health = collision.gameObject.GetComponent<Player.Stats.IDamagable>();

            if (health != null)
            {
                health.TakeDamage(damageOnHit, sender);
            }

            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            // Stop everything!
            rigidBody.velocity = Vector3.zero;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            trailRenderer.Clear();
        }
    }
}