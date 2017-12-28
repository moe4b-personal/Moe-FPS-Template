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

using Moe.GameFramework.UI;

using Moe.GameFramework;

namespace Moe.FPSTemplate
{
    public class StatusHUDMenu : HUDMenuElement
    {
        [SerializeField]
        protected Text health;
        public Text Health { get { return health; } }

        protected override void UpdateUIInternal(Player player)
        {
            UpdateHealth(player);
        }

        public virtual void UpdateHealth()
        {
            UpdateHealth(Level.PlayerInstance);
        }
        public virtual void UpdateHealth(Player player)
        {
            health.text = player.Health.ToString();
        }
    }
}