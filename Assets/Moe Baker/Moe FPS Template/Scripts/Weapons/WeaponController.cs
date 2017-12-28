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

namespace WeaponCore
{
	public partial class WeaponController
	{
        [SerializeField]
        protected BobModule bob;
        public BobModule Bob { get { return bob; } }
        [Serializable]
        public class BobModule : Module
        {
            [SerializeField]
            protected float multiplier = 0.2f;
            public float Multiplier { get { return multiplier; } }
            protected float scale = 1;
            public float Scale
            {
                get
                {
                    return scale;
                }
                set
                {
                    value = Mathf.Clamp01(value);

                    scale = value;
                }
            }

            public virtual void Process()
            {
                Link.transform.localPosition += Link.Player.Script.Controller.Headbob.Offset * scale * multiplier;
            }
        }

        public override void Init(IWeaponCorePlayer player)
        {
            bob.SetLink(this);
            bob.Init();

            base.Init(player);
        }

        protected override void EquipInternal(KitSlot newSlot)
        {
            base.EquipInternal(newSlot);

            Level.MenuInstance.HUD.Weapons.UpdateWeapon(kit[newSlot]);
        }

        public override void UpdateCurrent(WeaponUpdateData data)
        {
            base.UpdateCurrent(data);

            bob.Process();
        }
    }

    public partial interface IWeaponCorePlayer
    {
        Player Script { get; }
    }
}