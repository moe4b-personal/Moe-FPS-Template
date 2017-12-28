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
    [RequireComponent(typeof(Rigidbody))]
	public abstract class BaseProjectile : TypesData.TProjectile
    {
        [SerializeField]
        protected ProjectileData data;
        public ProjectileData Data { get { return data; } }
        public virtual void SetData(ProjectileData newData)
        {
            if (newData != null)
                data = newData;
        }

        [SerializeField]
        protected ImpactData impact = new ImpactData(1, 1f);
        public ImpactData Impact { get { return impact; } }
        [Serializable]
        public struct ImpactData
        {
            [NonSerialized]
            public int currentImpacts;

            [SerializeField]
            int maxImpacts;
            public int MaxImpacts { get { return maxImpacts; } }
            public bool MaxedImpacts { get { return currentImpacts >= maxImpacts; } }

            [SerializeField]
            float damageMultiplier;
            public float DamageMultiplier { get { return damageMultiplier; } }

            public ImpactData(int maxImpacts, float damageMultiplier)
            {
                currentImpacts = 0;

                this.maxImpacts = maxImpacts;
                this.damageMultiplier = damageMultiplier;
            }
        }

        public Vector3 StartPosition { get; protected set; }
        public float TraveledDistance { get; protected set; }

        public Rigidbody Rigidbody { get; protected set; }

        public virtual void Init()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            StartPosition = transform.position;

            StartCoroutine(CheckRangeProcedure());
        }

        protected virtual IEnumerator CheckRangeProcedure()
        {
            while (TraveledDistance < data.Range)
            {
                TraveledDistance = Vector3.Distance(transform.position, StartPosition);

                yield return new WaitForEndOfFrame();
            }

            OutOfRangeAction();
        }
        protected virtual void OutOfRangeAction()
        {
            Disable();
        }
        protected virtual void Disable()
        {
            Destroy(gameObject);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            ImpactAction(collision);
        }

        protected virtual void ImpactAction(Collision collision)
        {
            if (!impact.MaxedImpacts)
                ImpactDamage(collision);

            impact.currentImpacts++;
        }
        protected virtual void ImpactDamage(Collision collision)
        {
            DoDamage(collision.gameObject, Mathf.RoundToInt(data.Damage * impact.DamageMultiplier));
        }

        protected virtual void DoDamage(GameObject damaged)
        {
            DoDamage(damaged, data.Damage);
        }
        protected virtual void DoDamage(GameObject damaged, int damage)
        {
            Weapon.DoDamage(damaged, damage, null);
        }
    }

    public abstract partial class Projectile : BaseProjectile
    {

    }


    [Serializable]
    public class BaseProjectileData
    {
        [SerializeField]
        protected int damage = 100;
        public int Damage { get { return damage; } }

        [SerializeField]
        protected float range = 100;
        public float Range { get { return range; } }

        public BaseProjectileData(int damage, float range)
        {
            this.damage = damage;
            this.range = range;
        }
    }

    [Serializable]
    public class ProjectileData : BaseProjectileData
    {
        public ProjectileData(int damage, float range) : base(damage, range)
        {
            
        }
    }
}