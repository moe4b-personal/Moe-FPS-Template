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
	public partial class InGameMenu
	{
        [SerializeField]
        protected HUDMenu _HUD;
        public HUDMenu HUD { get { return _HUD; } }
    }
}