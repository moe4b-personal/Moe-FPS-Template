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
    public partial class BaseWeapon
    {
        static partial void DoDamageInternal(GameObject damaged, int damage)
        {
            Level.ObserverInstance.CmdDamage(damaged, damage);
        }
    }

	public partial class Weapon
	{
        
    }
}