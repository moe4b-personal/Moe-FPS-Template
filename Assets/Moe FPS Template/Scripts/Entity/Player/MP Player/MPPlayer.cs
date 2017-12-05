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

using Moe.Tools;

using Moe.FPSTemplate;

namespace Moe.GameFramework
{
	public partial class MPPlayer
    {
        [SerializeField]
        protected PlayerBody body;
        public PlayerBody Body { get { return body; } }
        public Animator Anim { get { return body.Anim; } }

        protected override void InitLocal()
        {
            base.InitLocal();

            body.Init();
        }

        protected override void UpdateLocal()
        {
            base.UpdateLocal();

            Body.UpdateAnimator();
        }

        protected override void LocalDie()
        {
            base.LocalDie();

            MPLevel.Current.SetStartCameraActive(true);

            MPLevel.MenuInstance.Respawn.Display();
        }
    }
}