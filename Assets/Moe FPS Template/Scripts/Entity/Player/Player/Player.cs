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

using ARFC;

using Moe.Tools;
using UnityEngine.Networking;

using WeaponCore;

namespace Moe.GameFramework
{
	public partial class Player : IWeaponCorePlayer
    {
        public FPController Controller { get; protected set; }

        [SerializeField]
        protected LayersData layers;
        public LayersData Layers { get { return layers; } }
        [Serializable]
        public class LayersData
        {
            [SerializeField]
            string local = "Local Player";
            public string Local { get { return local; } }

            [SerializeField]
            string remote = "Remote Player";
            public string Remote { get { return remote; } }
        }

        [SerializeField]
        protected WeaponController weapons;
        public WeaponController Weapons { get { return weapons; } }
        public WeaponNumberSwitcher WeaponsNumberSwitcher { get; protected set; }

        [SerializeField]
        protected AudioSource aud;
        public AudioSource Aud { get { return aud; } }

        Player IWeaponCorePlayer.Script { get { return this; } }

        public override void Init()
        {
            InitLayers();
            InitController();

            base.Init();
        }

        protected virtual void InitController()
        {
            Controller = GetComponent<FPController>();

            if (isLocalPlayer)
                Controller.Init();
            else
                Controller.InitRemote();
        }
        protected virtual void InitLayers()
        {
            GameTools.GameObject.SetLayer(gameObject, isLocalPlayer ? layers.Local : layers.Remote);
        }

        protected override void InitLocal()
        {
            base.InitLocal();

            weapons.Init(this);
            WeaponsNumberSwitcher = new WeaponNumberSwitcher(weapons);
            Level.MenuInstance.HUD.UpdateUI(this);

            Game.Pause.OnStateChanged += OnPause;
        }

        protected override void UpdateLocal()
        {
            if (Game.Pause.State == PauseState.Full)
                return;

            base.UpdateLocal();

            UpdateWeapons();

            ProcessSuicide();
        }

        protected override void UpdateInputModule()
        {
            if(Game.Pause.State == PauseState.None)
                base.UpdateInputModule();
        }

        protected virtual void UpdateWeapons()
        {
            weapons.UpdateCurrent(new WeaponUpdateData(This));

            WeaponsNumberSwitcher.Update();
        }

        public const KeyCode SuicideKey = KeyCode.BackQuote;
        protected virtual void ProcessSuicide()
        {
            if (Debug.isDebugBuild && Input.GetKeyDown(SuicideKey))
                Suicide();
        }
        public virtual void Suicide()
        {
            CmdDamage(gameObject, int.MaxValue);
        }

        protected override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if(isLocalPlayer)
                Level.MenuInstance.HUD.Status.UpdateHealth();
        }
        protected override void Die()
        {
            base.Die();

            if (isLocalPlayer)
                LocalDie();
            else
                RemoteDie();

            if (isServer)
                ServerDie();
        }
        protected virtual void LocalDie()
        {
            Level.MenuInstance.HUD.Hide();
        }
        protected virtual void RemoteDie()
        {

        }
        protected virtual void ServerDie()
        {
            NetworkServer.Destroy(gameObject);
        }

        public virtual void SpawnProjectile(GameObject prefab, uint shooterID, Vector3 position, Quaternion rotation, float force, ProjectileData data)
        {
            var hash = prefab.GetComponent<NetworkIdentity>().assetId;

            CmdSpawnProjectile(hash, shooterID, position, rotation, force, GameTools.Serialization.Binary.GetBytes(data));
        }
        [Command]
        protected virtual void CmdSpawnProjectile(NetworkHash128 prefabHash, uint shooterID, Vector3 position, Quaternion rotation, float force, byte[] dataBytes)
        {
            var data = GameTools.Serialization.Binary.GetObject<ProjectileData>(dataBytes);

            GameObject prefab = ClientScene.prefabs[prefabHash];
            
            var instance = Instantiate(prefab, position, rotation).GetComponent<Projectile>();
            NetworkServer.Spawn(instance.gameObject);

            instance.SetData(data);

            RpcInitProjectile(instance.gameObject, shooterID);

            instance.Init();

            instance.Rigidbody.AddForce(instance.transform.forward * force, ForceMode.VelocityChange);
        }
        [ClientRpc]
        protected virtual void RpcInitProjectile(GameObject projectile, uint shooterID)
        {
            projectile.GetComponent<Projectile>().Init();

            GameTools.GameObject.SetCollision(projectile, ClientScene.FindLocalObject(new NetworkInstanceId(shooterID)), false);
        }

        protected virtual void OnPause(PauseState state)
        {
            BaseInputModule.Clear();
        }
    }
}

namespace WeaponCore
{
    public partial struct WeaponUpdateData
    {
        public WeaponUpdateData(Moe.GameFramework.Player player)
        {
            this.Use = player.BaseInputModule.Shoot;
            this.Aim = player.BaseInputModule.Aim;

            this.Switch = player.BaseInputModule.SwitchDirection;

            this.Reload = player.BaseInputModule.Reload;

            this.Sprint = player.Controller.CurrentState == ControllerState.Sprinting;

            this.Sway = player.Controller.InputModule.Look;
        }
    }
}