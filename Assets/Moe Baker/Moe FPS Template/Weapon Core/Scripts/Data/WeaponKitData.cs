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
	public abstract class BaseWeaponKitData : ScriptableObject
	{
		[SerializeField]
        protected WeaponKit kit;
        public WeaponKit Kit { get { return kit; } }
    }

    [CreateAssetMenu(menuName = Weapon.MenuPath + "Kit")]
    public class WeaponKitData : BaseWeaponKitData
    {

    }
}