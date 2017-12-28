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

using ARFC;

namespace Moe.FPSTemplate
{
    [CreateAssetMenu(menuName = FPControllerInputModule.MenuPath + "Bind Module")]
    public partial class BindFPControllerInputModule : FPControllerInputModule
    {
        [SerializeField]
        RawKeyBiningAxis walkAxis;

        [SerializeField]
        RawKeyBiningAxis strafeAxis;

        [SerializeField]
        KeyBindingAccessor jumpBind;

        [SerializeField]
        KeyBindingAccessor sprintBind;

        [SerializeField]
        KeyBindingAccessor crouchBind;

        [SerializeField]
        KeyBindingAccessor proneBind;

        public override void UpdateInput()
        {
            base.UpdateInput();

            look.x = Input.GetAxis("Mouse X");
            look.y = Input.GetAxis("Mouse Y");

            walkAxis.Update();
            strafeAxis.Update();

            movement = new Vector2(strafeAxis.RawValue, walkAxis.RawValue);

            jump = Game.Control.GetKeyBindDown(jumpBind);

            sprint = Game.Control.GetKeyBind(sprintBind);

            crouch = Game.Control.GetKeyBindDown(crouchBind);
            prone = Game.Control.GetKeyBindDown(proneBind);
        }
    }
}