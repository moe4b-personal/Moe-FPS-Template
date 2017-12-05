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

namespace WeaponCore
{
	public partial class ExplosiveProjectile
	{
        protected override IEnumerator Procedure()
        {
            if (!isServer)
                return null;

            return base.Procedure();
        }

        protected override void Explode()
        {
            if(isServer)
            {
                base.Explode();

                if (isServer)
                    RpcExplode();
            }
        }

        [ClientRpc]
        protected void RpcExplode()
        {
            if (isServer)
                return;

            explosion.Explode();
            Interactability = false;
        }
    }
}