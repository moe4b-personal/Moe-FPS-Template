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
	public partial class RangedSharpShooterWeapon
	{
        public partial class AimModule
        {
            [SerializeField]
            protected float bobScale = 0.2f;
            public float BobScale { get { return bobScale; } }

            public override void Toggle()
            {
                base.Toggle();

                Weapon.Controller.Bob.Scale = isOn ? bobScale : 1f;
            }

            public partial class StateMachinesModule
            {
                protected override void OnStart()
                {
                    base.OnStart();

                    Level.MenuInstance.HUD.Crosshair.Visibility = false;
                }

                protected override void OnEnd()
                {
                    base.OnEnd();

                    Level.MenuInstance.HUD.Crosshair.Visibility = true;
                }
            }
        }
    }
}