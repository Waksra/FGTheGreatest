using System;
using System.Collections;
using UnityEngine;

namespace Actor_Components
{
    public class ShootingController : MonoBehaviour
    {
        [SerializeField] private ObjectPooler.PooledObjects objectToFire;
        [SerializeField] private Transform projectileOrigin;

        private void Awake()
        {
            if (projectileOrigin == null)
                projectileOrigin = GetComponent<Transform>();
        }

        public void Shoot()
        {
            GameObject projectile = ObjectPooler.SharedInstance.GetPooledObject(objectToFire);

            projectile.transform.up = projectileOrigin.up;
            projectile.transform.position = projectileOrigin.position;
            projectile.SetActive(true);
        }

        public void ChangeProjectile(ObjectPooler.PooledObjects newProjectile, float duration)
        {
            StartCoroutine(ResetProjectileType(objectToFire, duration));
            ChangeProjectile(newProjectile);
        }

        public void ChangeProjectile(ObjectPooler.PooledObjects newProjectile)
        {
            objectToFire = newProjectile;
        }

        private IEnumerator ResetProjectileType(ObjectPooler.PooledObjects projectile, float delay)
        {
            yield return new WaitForSeconds(delay);
            ChangeProjectile(projectile);
        }
    }
}
