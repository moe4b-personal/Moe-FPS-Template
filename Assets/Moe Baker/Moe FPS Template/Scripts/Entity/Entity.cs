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

namespace Moe.GameFramework
{
    public partial class Entity
    {
        [Command]
        public virtual void CmdDamage(GameObject gameObject, int damage)
        {
            if (gameObject == null)
            {
                Debug.LogError("Trying To Damage Non Existing Game Object");

                return;
            }

            var entity = gameObject.GetComponent<Entity>();

            if (entity == null)
            {

            }
            else
            {
                entity.RpcTakeDamage(damage);
            }
        }

        [ClientRpc]
        public virtual void RpcTakeDamage(int damage)
        {
            TakeDamage(damage);
        }
        protected virtual void TakeDamage(int damage)
        {
            
        }
    }
}