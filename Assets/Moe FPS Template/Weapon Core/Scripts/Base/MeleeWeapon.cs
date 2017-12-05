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
	public abstract partial class BaseMeleeWeapon : RangedWeapon
	{
        private MeleeWeapon This { get { return this as MeleeWeapon; } }

        public override WeaponType Type { get { return base.Type | WeaponType.Melee; } }

        [SerializeField]
        protected MeleeWeapon.CastModule cast;
        public MeleeWeapon.CastModule Cast { get { return cast; } }
        [Serializable]
        public abstract class BaseCastModule : CastModule<MeleeWeapon>
        {
            protected AnimatorEventRewind eventRewind;
            public AnimatorEventRewind EventRewind { get { return eventRewind; } }

            public override void Init(MeleeWeapon weapon)
            {
                base.Init(weapon);

                eventRewind = Weapon.anim.GetComponent<AnimatorEventRewind>();

                eventRewind.AddHandler("Contact", Process);
            }
        }

        protected override void AddModules()
        {
            base.AddModules();

            Modules.Add(cast);
        }
    }

    public partial class MeleeWeapon : BaseMeleeWeapon
    {
        [Serializable]
        public partial class CastModule : BaseCastModule
        {
            
        }
    }
}