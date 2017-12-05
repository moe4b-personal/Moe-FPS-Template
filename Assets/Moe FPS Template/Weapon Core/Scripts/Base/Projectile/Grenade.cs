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
	public abstract class BaseGrenade : ExplosiveProjectile
	{
        [SerializeField]
        protected TickData tick = new TickData(3.5f);
        public TickData Tick { get { return tick; } }
        [Serializable]
        public struct TickData
        {
            [SerializeField]
            float delay;
            public float Delay { get { return delay; } }

            [SerializeField]
            SoundSet _SFX;
            public SoundSet SFX { get { return _SFX; } }

            public TickData(float delay)
            {
                this.delay = delay;
                this._SFX = null;
            }
        }

        [SerializeField]
        protected OnImpactAction onImpact = OnImpactAction.Nothing;
        public OnImpactAction OnImpact { get { return onImpact; } }

        protected override IEnumerator Procedure()
        {
            var time = delay.Detonation;

            while (true)
            {
                yield return new WaitForSeconds(tick.Delay);

                TickAction();

                time -= tick.Delay;

                if (time <= 0f)
                    break;
            }

            ProcedureEndAction();
        }
        protected virtual void TickAction()
        {
            explosion.Aud.PlayOneShot(tick.SFX.RandomClip);
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            if (onImpact == OnImpactAction.Explode)
                Explode();
        }

        public enum OnImpactAction
        {
            Nothing, Explode
        }
    }

    public partial class Grenade : BaseGrenade
    {

    }
}