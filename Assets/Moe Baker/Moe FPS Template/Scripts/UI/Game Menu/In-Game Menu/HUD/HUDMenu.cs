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
    public class HUDMenu : BaseMenuUI
    {
        [SerializeField]
        protected StatusHUDMenu status;
        public StatusHUDMenu Status { get { return status; } }

        [SerializeField]
        protected WeaponsHUDMenu weapons;
        public WeaponsHUDMenu Weapons { get { return weapons; } }

        [SerializeField]
        protected Crosshair crosshair;
        public Crosshair Crosshair { get { return crosshair; } }

        public virtual void UpdateUI()
        {
            UpdateUI(Level.PlayerInstance);
        }
        public virtual void UpdateUI(Player player)
        {
            if (player)
            {
                Display();

                status.UpdateUI(player);
                weapons.UpdateUI(player);
            }
            else
                Hide();
        }
    }
}