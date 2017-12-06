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

using Moe.GameFramework;

namespace WeaponCore
{
	public partial class DepletableWeapon
	{
        protected override void UpdateAmmoUI()
        {
            base.UpdateAmmoUI();

            Level.MenuInstance.HUD.Weapons.UpdateAmmo(this);
        }

        public partial class ShootModule
        {
            [SerializeField]
            protected float crosshairKick = 25f;
            public float CrosshairKick { get { return crosshairKick; } }

            protected override void Process()
            {
                base.Process();

                Level.MenuInstance.HUD.Crosshair.DistanceValue = crosshairKick;
            }
        }
    }
}