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

namespace Moe.GameFramework
{
	public partial class ControlManager
	{
        public partial class ActionKeysData
        {
            [SerializeField]
            protected KeyCode respawn = KeyCode.F;
            public KeyCode Respawn { get { return respawn; } }
        }
    }
}