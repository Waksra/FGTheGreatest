using System;
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
    }
}
