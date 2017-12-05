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
	public partial class Level
	{
		public partial class PlayersModule<TPlayer>
        {
            public override void Add(TPlayer element)
            {
                base.Add(element);

                element.OnDeath += delegate () { list.Remove(element); };
            }
        }
	}
}