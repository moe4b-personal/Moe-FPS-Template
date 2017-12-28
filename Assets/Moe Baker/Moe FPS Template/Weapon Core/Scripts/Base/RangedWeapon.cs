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
	public abstract partial class BaseRangedWeapon : Weapon
	{
        private RangedWeapon This { get { return this as RangedWeapon; } }

        public override WeaponType Type { get { return WeaponType.Ranged; } }

        [SerializeField]
        protected float range = 100;
        public float Range { get { return range; } }

        [Serializable]
        public abstract class BaseCastModule<TWeapon> : Module<TWeapon>
            where TWeapon : RangedWeapon
        {
            [SerializeField]
            protected LayerMask mask = Physics.AllLayers;
            public LayerMask Mask { get { return mask; } }

            [SerializeField]
            protected QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.UseGlobal;
            public QueryTriggerInteraction TriggerInteraction { get { return triggerInteraction; } }

            [SerializeField]
            protected Transform point;
            public Transform Point { get { return point; } }

            public RaycastHit? Hit { get; protected set; }

            public virtual void Process()
            {
                RaycastHit hit;

                if (Cast(out hit))
                    ProcessHit(hit);
                else
                    Hit = null;
            }
            protected virtual bool Cast(out RaycastHit hit)
            {
                return Physics.Raycast(point.transform.position, point.forward, out hit, Weapon.Range, mask, triggerInteraction);
            }
            protected virtual void ProcessHit(RaycastHit hit)
            {
                Hit = hit;

                Weapon.DoDamage(hit.collider.gameObject);
            }
            protected virtual void ProcessMiss()
            {
                Hit = null;
            }
        }
    }

    public abstract partial class RangedWeapon : BaseRangedWeapon
    {
        [Serializable]
        public abstract partial class CastModule<TWeapon> : BaseCastModule<TWeapon>
            where TWeapon : RangedWeapon
        {

        }
    }
}