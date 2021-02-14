using UnityEngine;

namespace PuppetMaster
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float shotForce = 100;
        [SerializeField] private float lifeTime = 3;
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private TrailRenderer trailRenderer;

        private float timeAlive = 0;
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

        private void OnEnable()
        {
            // Go shooting in a direction
            rigidBody.AddForce(transform.forward * shotForce, ForceMode.Impulse);
        }

        private void OnDisable()
        {
            // Reset all the information we will need when we re-enable
            timeAlive = 0;
            rigidBody.velocity = Vector3.zero;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            trailRenderer.Clear();
        }

        private void Update()
        {
            // Disable this object after it's lived it's life duration
            if (timeAlive > lifeTime)
            {
                gameObject.SetActive(false);
            }
            else timeAlive += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            // If there is a damageable component, damage it.
            var health = other.gameObject.GetComponent<Player.Stats.IDamagable>();
            if (health != null) health.TakeDamage(damageOnHit, sender);

            // Disable this projectile
            gameObject.SetActive(false);
        }
    }
}