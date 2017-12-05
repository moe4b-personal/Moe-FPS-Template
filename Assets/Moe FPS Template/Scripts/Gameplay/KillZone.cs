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

namespace Moe.FPSTemplate
{
	public class KillZone : Entity
	{
		protected virtual void OnTriggerEnter(Collider col)
        {
            var entity = col.gameObject.GetComponent<Entity>();

            if (entity)
                CmdDamage(entity.gameObject, int.MaxValue);
        }
	}
}