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
	public partial class ProjectileWeapon
	{
		public partial class ProjectileModule
        {
            protected override void Instantiate()
            {
                MPLevel.PlayerInstance.SpawnProjectile(prefab, MPLevel.PlayerInstance.netId.Value, point.position, point.rotation, Weapon.force, GetProjectileData());
            }
        }
	}
}