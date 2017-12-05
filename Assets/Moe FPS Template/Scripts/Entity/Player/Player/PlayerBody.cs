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
    public class PlayerBody : MonoBehaviour
    {
        public Animator Anim { get; protected set; }

        MPPlayer Player { get { return MPLevel.PlayerInstance; } }
        FPController Controller { get { return Player.Controller; } }

        public bool Crouching
        {
            set
            {
                Anim.SetBool("Crouching", value);
            }
        }
        public bool Proning
        {
            set
            {
                Anim.SetBool("Proning", value);
            }
        }

        public virtual void Init()
        {
            GetComponents();
        }

        public virtual void GetComponents()
        {
            Anim = GetComponent<Animator>();
        }

        public virtual void UpdateAnimator()
        {
            Vector2 move = Controller.Movement.Speed.Vector;

            move /= Controller.Movement.Speed.Current.Max;

            Crouching = Controller.CurrentState == ControllerState.Crouching;
            Proning = Controller.CurrentState == ControllerState.Proning;

            if (Controller.CurrentState == ControllerState.Sprinting)
                move *= 2;

            Anim.SetFloat("Walk", move.y);
            Anim.SetFloat("Strafe", move.x);
        }
    }
}