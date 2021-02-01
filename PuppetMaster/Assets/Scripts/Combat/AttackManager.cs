using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Input;
using Player.Stats;

namespace PuppetMaster
{
    public class AttackManager : MonoBehaviour, IActionInputReciever
    {
        // FIXME: move this input system over to the

        // When the player sends input in from the mouse
        // in a specified direction send the player shooting
        // in that direction.

        public new Rigidbody rigidbody;
        private Transform _transform;

        public float turnSpeed = 30;
        public float attackForce = 30;
        public float attackRange = 100;
        public AnimationCurve attackSpeed;

        public float backUpAmount = 5;

        public float damageRange = 1.5f;

        public int damage = 100;

        private Vector3 lookDirection;

        // Have a list of targets and attack all of them
        private List<Transform> targets = new List<Transform>();

        private bool targeting = false;
        private bool attacking = false;
        private Transform hoveringOver;

        private List<GameObject> targetVisualizers = new List<GameObject>();

        public GameObject targetPrefab;

        private void Start()
        {
            // Cache some values
            if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
        }

        private void FixedUpdate()
        {
            if (attacking) return;

            if (targeting)
            {
                //FIX ME: Switch to ray casting in the direction of the mouse.
                // TODO we should ray cast from the last target, or ourself if there is no last target.

                // If attacking we should add things we highlight with the mouse into the targets list
                var allCharacters = FindObjectsOfType<CharacterStats>(); // OBVIOUSLY WE WILL REMOVE THIS

                var startTargetCount = targets.Count;
                bool hovering = false;

                foreach (var item in allCharacters)
                {
                    if (item.transform == _transform)
                        continue;

                    // Check if the cursor is over the object
                    var offset = Convienience.Utilities.GetMouseOffsetFromObject(item.transform, Screen.width);
                    var distance = Vector3.Distance(Vector3.zero, offset);

                    if (distance <= 0.03f)
                    {
                        // Check to see if the cursor is hovering or just passing
                        hovering = true;

                        if (hoveringOver != item.transform)
                        {
                            hoveringOver = null;
                        }
                        else
                        {
                            continue;
                        }

                        // Add the target to the list
                        CreateTarget(item.transform);
                        targets.Add(item.transform);

                        hoveringOver = item.transform;
                    }
                }

                if (startTargetCount == targets.Count && !hovering)
                    hoveringOver = null;
            }

            // Look at cursor
            if (Time.frameCount % 3 == 0)
            {
                LookAtCursor();
            }
        }

        /// <summary>
        /// Create a target icon over the object being attacked.
        /// NOTE: This really should just be handled by another script, this one has enough to worry about.
        /// </summary>
        private void CreateTarget(Transform parent)
        {
            float rotationRange = 15.0f;

            var obj = Instantiate(targetPrefab,
                parent.transform.position + Vector3.up + (Vector3)(Random.insideUnitCircle * 0.1f),
                Quaternion.Euler(new Vector3(0, 0, Random.Range(-rotationRange, rotationRange))),
                parent);

            targetVisualizers.Add(obj);
        }

        private void LookAtCursor()
        {
            lookDirection = Convienience.Utilities.GetMouseOffsetFromObject(_transform, 1.1f);

            UpdateLookRotation();
        }

        private void UpdateLookRotation()
        {
            var targetDir = lookDirection + _transform.position;
            var localTarget = _transform.InverseTransformPoint(targetDir);

            var angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            var eulerAngleVelocity = new Vector3(0, angle, 0);
            var deltaRotation = Quaternion.Euler(eulerAngleVelocity * turnSpeed * Time.fixedDeltaTime);
            rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        }

        public void OnFire1()
        {
            // Started attack input
            targeting = true;

            // We should (FIXME: TRY TO) slow time here.
            TimeScaleManager.SetTimeScale(0.1f);
        }

        public void OnFire2()
        {
            targeting = false;

            StartCoroutine(AttackTargets());
        }

        /// <summary>
        /// Move towards each target and attack them.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AttackTargets()
        {
            if (attacking) yield break;
            attacking = true;

            yield return null;
            TimeScaleManager.SetTimeScale(1);

            if (targets.Count > 0)
            {
                targets.Reverse();

                for (int i = targets.Count - 1; i >= 0; i--)
                {
                    Transform target = targets[i];
                    var startPosition = _transform.position;
                    lookDirection = (target.position - _transform.position);
                    UpdateLookRotation();

                    float dist = Vector3.Distance(_transform.position, target.transform.position);
                    float totalDist = dist;

                    Vector3 velocity = Vector3.zero;

                    while (true)
                    {
                        if (target == null)
                        {
                            rigidbody.velocity = Vector3.zero;
                            break;
                        }

                        // Time stuff
                        var timeValue = Mathf.Clamp(attackSpeed.Evaluate(1 - (dist / totalDist)), 0.1f, 1.0f);
                        TimeScaleManager.SetTimeScale(timeValue);

                        // Move and damaging
                        if (dist > damageRange)
                        {
                            // Move towards target
                            velocity = ((target.position - _transform.position) * attackForce) * timeValue;
                            rigidbody.velocity = velocity;
                        }
                        else
                        {
                            // Damage target
                            var otherHealth = target.gameObject.GetComponent<IDamagable>();
                            otherHealth.TakeDamage(damage, _transform);

                            // Back away from target hit for a possible rebound attack
                            rigidbody.velocity = -velocity * backUpAmount * 0.1f;

                            yield return new WaitForSeconds(0.05f);

                            // Stop for the most part
                            while (Vector3.Distance(rigidbody.velocity, Vector3.zero) > 0.5f)
                            {
                                rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.zero, attackForce * Time.deltaTime);
                                yield return null;
                            }

                            rigidbody.velocity = Vector3.zero;

                            // Remove this item from the list
                            for (int j = 0; j < targetVisualizers.Count; j++)
                            {
                                if (target == targetVisualizers[j].transform.parent)
                                {
                                    // We could be pooling these
                                    Destroy(targetVisualizers[j]);
                                    targetVisualizers.RemoveAt(j);
                                    break;
                                }
                            }
                            targets.RemoveAt(i);

                            break;
                        }
                        yield return null;

                        dist = Vector3.Distance(_transform.position, target.transform.position);
                    }

                    TimeScaleManager.SetTimeScale(1);
                }

                targets.Clear();
            }
            else
            {
                // Ended attack input
                LookAtCursor();

                lookDirection = Convienience.Utilities.GetMouseOffsetFromObject(_transform, 1.1f);

                // Send the player out in the direction of the attack
                rigidbody.AddForce(lookDirection * attackForce, ForceMode.Impulse);
            }

            attacking = false;
        }
    }
}