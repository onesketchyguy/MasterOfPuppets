using UnityEngine;

namespace PuppetMaster
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float shotForce = 100;
        [SerializeField] private Rigidbody rigidBody;

        private float timeAlive = 0;

        private void OnEnable()
        {
            // Go flying in a direction
            rigidBody.AddForce(transform.forward * shotForce, ForceMode.Impulse);

            timeAlive = 0;
        }

        private void Update()
        {
            if (timeAlive > 3)
            {
                gameObject.SetActive(false);
            }
            else timeAlive -= Time.deltaTime;
        }

        private void OnDisable()
        {
            // Stop everything!
            rigidBody.velocity = Vector3.zero;
            transform.position = Vector3.zero;
        }
    }
}