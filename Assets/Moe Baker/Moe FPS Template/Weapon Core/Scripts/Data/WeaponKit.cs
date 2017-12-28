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
    [Serializable]
	public abstract class BaseWeaponKit : WeaponKitTemplate<WeaponData>
	{
		
	}

    [Serializable]
    public partial class WeaponKit : BaseWeaponKit
    {

    }
}