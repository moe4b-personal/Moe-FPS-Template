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

using Moe.FPSTemplate;

namespace Moe.GameFramework.UI
{
	public partial class MPInGameMenu
	{
        [SerializeField]
        protected RespawnMenu respawn;
        public RespawnMenu Respawn { get { return respawn; } }
    }
}