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
	public abstract class BaseExplosiveProjectile : Projectile
	{
		[SerializeField]
        protected Explosion explosion;
        public Explosion Explosion { get { return explosion; } }

        [SerializeField]
        protected DelayData delay = new DelayData(4f, 5f);
        public DelayData Delay { get { return delay; } }
        [Serializable]
        public struct DelayData
        {
            [SerializeField]
            float detonation;
            public float Detonation { get { return detonation; } }

            [SerializeField]
            float explosion;
            public float Explosion { get { return explosion; } }

            public DelayData(float detonation, float explosion)
            {
                this.detonation = detonation;
                this.explosion = explosion;
            }
        }

        [SerializeField]
        protected float damageFallOff = 10f;
        public float DamageFallOff { get { return damageFallOff; } }

        [SerializeField]
        protected GameObject mesh;
        public GameObject Mesh { get { return mesh; } }
        public virtual bool Interactability
        {
            set
            {
                Rigidbody.isKinematic = !value;
                mesh.SetActive(value);
            }
        }

        public override void Init()
        {
            base.Init();

            Coroutine = new AutoCoroutine(this, Procedure);
            Coroutine.Start();

            Interactability = true;

            explosion.Init();
            explosion.OnAddForce += ProcessExplosion;
            explosion.Ignores.Add(Rigidbody);
        }

        public AutoCoroutine Coroutine { get; protected set; }
        protected virtual IEnumerator Procedure()
        {
            yield return new WaitForSeconds(delay.Detonation);

            ProcedureEndAction();
        }
        protected virtual void ProcedureEndAction()
        {
            Explode();
        }

        protected override void OutOfRangeAction()
        {
            Explode();
        }

        protected virtual void Explode()
        {
            Coroutine.End();

            Interactability = false;

            explosion.Explode();

            Invoke("Disable", delay.Explosion);
        }

        protected virtual void ProcessExplosion(Rigidbody rigidbody, float distance)
        {
            var damage = GetDamageAtDistance(distance);

            if (damage > 0)
                DoDamage(rigidbody.gameObject, damage);
        }

        public virtual int GetDamageAtDistance(float distance)
        {
            if (distance * damageFallOff >= data.Damage)
                return 0;

            return Mathf.RoundToInt(data.Damage - (distance * damageFallOff));
        }
    }

    public abstract partial class ExplosiveProjectile : BaseExplosiveProjectile
    {

    }
}