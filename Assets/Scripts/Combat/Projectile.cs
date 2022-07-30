using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        private Health _target = null;
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool homing = false;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifetime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2f;

        private GameObject _instigator;
        private float _damage = 5f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null) return;

            if (homing && _target.IsDead())
            {
                transform.LookAt(GetAimLocation());

            }
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

        }
        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this._instigator = instigator;
            this._target = target;
            this._damage = damage;
            
            Destroy(gameObject, maxLifetime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return _target.transform.position;
            }

            return _target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
           
            if (other.GetComponent<Health>()!= _target) return;
            if(_target.IsDead()) return;
            _target.TakeDamage(_instigator, _damage);
            speed = 0;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (var toDestroy in destroyOnHit)
            {
                Destroy(toDestroy, lifeAfterImpact);
            }
            Destroy(gameObject);
            
        }
    }
}

