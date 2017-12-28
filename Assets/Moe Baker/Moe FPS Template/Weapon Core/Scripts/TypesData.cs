using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
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
    public abstract class BaseTypesData
    {
        public abstract class TWeapon : MonoBehaviour
        {

        }

        public abstract class TProjectile : MonoBehaviour
        {

        }
    }

    public abstract partial class TypesData : BaseTypesData
    {
        
    }
}