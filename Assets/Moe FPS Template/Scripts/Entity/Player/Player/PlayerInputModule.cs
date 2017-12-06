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

namespace Moe.GameFramework
{
	public partial class PlayerInputModule
	{
        [SerializeField]
        KeyBindingAccessor shootBind;
        protected bool shoot;
        public bool Shoot { get { return shoot; } }

        [SerializeField]
        KeyBindingAccessor aimBind;
        protected bool aim;
        public bool Aim { get { return aim; } }

        [SerializeField]
        protected int switchDirection;
        public int SwitchDirection { get { return switchDirection; } }

        [SerializeField]
        KeyBindingAccessor reloadBind;
        protected bool reload;
        public bool Reload { get { return reload; } }

        public override void UpdateInput()
        {
            base.UpdateInput();

            shoot = Game.Control.GetKeyBind(shootBind);

            aim = Game.Control.GetKeyBindDown(aimBind);

            reload = Game.Control.GetKeyBindDown(reloadBind);

            switchDirection = Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel") * -10);
        }

        public override void Clear()
        {
            base.Clear();

            shoot = false;
            aim = false;
            switchDirection = 0;
            reload = false;
        }
    }
}