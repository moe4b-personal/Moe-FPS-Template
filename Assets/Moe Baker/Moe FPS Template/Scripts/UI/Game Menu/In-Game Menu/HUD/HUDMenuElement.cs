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
using Moe.GameFramework.UI;

namespace Moe.FPSTemplate
{
    public abstract class HUDMenuElement : BaseMenuUI
    {
        public virtual void UpdateUI()
        {
            UpdateUI(Level.PlayerInstance);
        }
        public virtual void UpdateUI(Player player)
        {
            if (player)
                UpdateUIInternal(player);
            else
                ClearUI();
        }

        protected abstract void UpdateUIInternal(Player player);
        public virtual void ClearUI()
        {
            Hide();
        }
    }
}