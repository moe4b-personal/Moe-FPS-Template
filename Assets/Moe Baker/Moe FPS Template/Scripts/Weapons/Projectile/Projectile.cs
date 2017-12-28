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
using Moe.GameFramework;

namespace WeaponCore
{
	public partial class Projectile
	{
        public override void Init()
        {
            if (isServer)
                base.Init();
            else
                Rigidbody = GetComponent<Rigidbody>();
        }

        protected override IEnumerator CheckRangeProcedure()
        {
            if (!isServer)
                return null;

            return base.CheckRangeProcedure();
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            if(isServer)
                base.OnCollisionEnter(collision);
        }

        protected override void Disable()
        {
            if (isServer)
                NetworkServer.Destroy(gameObject);
        }
    }
}