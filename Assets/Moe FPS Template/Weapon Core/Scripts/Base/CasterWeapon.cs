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

namespace WeaponCore
{
	public abstract partial class BaseCasterWeapon : DepletableWeapon
	{
        private CasterWeapon This { get { return this as CasterWeapon; } }

        public override WeaponType Type { get { return base.Type | WeaponType.Caster; } }

        [SerializeField]
        protected CasterWeapon.CastModule cast;
        public CasterWeapon.CastModule Cast { get { return cast; } }
        [Serializable]
        public abstract class BaseCastModule : CastModule<CasterWeapon>
        {
            public override void Init(CasterWeapon weapon)
            {
                base.Init(weapon);

                Weapon.OnShoot += Process;
            }
        }

        protected override void AddModules()
        {
            base.AddModules();

            Modules.Add(cast);
        }
    }

    public abstract partial class CasterWeapon : BaseCasterWeapon
    {
        [Serializable]
        public partial class CastModule : BaseCastModule
        {

        }
    }
}