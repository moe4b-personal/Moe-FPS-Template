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

using WeaponCore;

namespace Moe.FPSTemplate
{
    [Serializable]
    public class WeaponsHUDMenu : HUDMenuElement
    {
        [SerializeField]
        Image icon;
        public Image Icon { get { return icon; } }

        [SerializeField]
        new protected Text name;
        public Text Name { get { return name; } }

        [SerializeField]
        protected Text ammo;
        public Text Ammo { get { return ammo; } }

        public const string EmptyAmmoText = "--";
        public const string NoAmmoText = EmptyAmmoText + "/" + EmptyAmmoText;

        protected override void UpdateUIInternal(Player player)
        {
            UpdateWeapon(player.Weapons.CurrentWeapon);
        }

        public virtual void UpdateWeapon(Weapon weapon)
        {
            if(weapon)
            {
                name.text = weapon.Data.name;
                icon.sprite = weapon.Data.Sprite;

                UpdateAmmo(weapon.TypeCast.Depletable.Value);
            }
            else
            {
                name.text = "";
                icon.sprite = null;
                UpdateAmmo(null);
            }
        }

        public virtual void UpdateAmmo(DepletableWeapon weapon)
        {
            if(weapon == null)
                ammo.text = NoAmmoText;
            else
                ammo.text = (weapon.Reload.Processing ? EmptyAmmoText : weapon.Ammo.Clip.Value.ToString()) + "/" + weapon.Ammo.Reserve.Value;
        }
    }
}