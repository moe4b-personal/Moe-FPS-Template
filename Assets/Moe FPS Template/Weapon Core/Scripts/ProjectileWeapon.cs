using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using Moe.Tools;

namespace WeaponCore
{
	public abstract partial class BaseProjectileWeapon : DepletableWeapon
	{
        private ProjectileWeapon This { get { return this as ProjectileWeapon; } }

        public override WeaponType Type { get { return base.Type | WeaponType.Projectile; } }

        [SerializeField]
        protected float force = 100f;
        public float Force { get { return force; } }

        [SerializeField]
        protected ProjectileWeapon.ProjectileModule projectile;
        public ProjectileWeapon.ProjectileModule Projectile { get { return projectile; } }
        [Serializable]
        public abstract class BaseProjectileModule : Module<ProjectileWeapon>
        {
            [SerializeField]
            protected Transform point;
            public Transform Point { get { return point; } }

            [SerializeField]
            protected GameObject prefab;
            public GameObject Prefab { get { return prefab; } }

            public override void Init(ProjectileWeapon weapon)
            {
                base.Init(weapon);

                Weapon.OnShoot += Instantiate;
            }

            protected virtual Projectile GetInstance(Vector3 position, Quaternion rotation)
            {
                return Object.Instantiate(prefab, position, rotation).GetComponent<Projectile>();
            }

            protected virtual void Instantiate()
            {
                var projectile = GetInstance(point.position, point.rotation);

                InitProjectile(projectile);
            }

            protected virtual void InitProjectile(Projectile projectile)
            {
                GameTools.GameObject.SetCollision(projectile.gameObject, Weapon.Player.gameObject, false);

                projectile.SetData(GetProjectileData());
            }

            protected virtual ProjectileData GetProjectileData()
            {
                return new ProjectileData(Weapon.RandomDamage, Weapon.range);
            }
        }

        protected override void AddModules()
        {
            base.AddModules();

            Modules.Add(projectile);
        }
    }

    public partial class ProjectileWeapon : BaseProjectileWeapon
    {
        [Serializable]
        public partial class ProjectileModule : BaseProjectileModule
        {

        }
    }
}