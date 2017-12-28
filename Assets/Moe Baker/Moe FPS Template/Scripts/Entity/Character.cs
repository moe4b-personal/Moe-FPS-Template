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

namespace Moe.GameFramework
{
    public partial class Character
    {
        [SerializeField]
        [SyncVar]
        protected int health = 100;
        public int Health { get { return health; } }

        protected override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (health == 0)
                return;

            if (health < damage)
                health = 0;
            else
                health -= damage;

            if (health == 0)
                Die();
        }

        public event Action OnDeath;
        protected virtual void Die()
        {
            if (OnDeath != null)
                OnDeath();
        }
    }
}