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
	public abstract partial class BaseGun : CasterWeapon
	{
        public override WeaponType Type { get { return base.Type | WeaponType.Gun; } }

        [SerializeField]
        protected GunType gunType = GunType.Pistol;
        public GunType GunType { get { return gunType; } }
    }

    public partial class Gun : BaseGun
    {

    }

    public enum GunType
    {
        Pistol, AssaultRifle, SniperRifle, SubMachineGun, LightMachineGun, MarksmanRifle, Shotgun
    }
}