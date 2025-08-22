﻿using Exiled.API.Enums;
using Exiled.API.Features.Items;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using PlayerRoles.Voice;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace FacilityManagement
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = false;
        public bool Debug { get; set; } = false;

        [Description("Make infinite ammo for weapon.")]
        public List<ItemType> InfiniteAmmo { get; set; } = new()
        {
            ItemType.ParticleDisruptor,
        };

        [Description("Make infinite ammo for weapon.")]
        public float EnergyMicroHid { get; set; } = 1f;

        [Description("Make infinite ammo for weapon.")]
        public float EnergyRadio { get; set; } = 1f;

        [Description("Sets the config of Generator")]
        public GeneratorBuild CustomGenerator { get; set; } = new()
        {
            UnlockCooldown = 0.1f,
            LeverDelay = 0.5f,
            DoorPanelCooldown = 0.9f,
            InteractionCooldown = 0,
            DeactivationTime = 125,
            RequiredPermission = KeycardPermissions.ArmoryLevelOne,
        };

        [Description("Sets the time for Lift to teleport")]
        public Dictionary<ElevatorType, float> LiftMoveDuration { get; set; } = new();

        [Description(@"Custom intercom content. If there's no specific content, then the default client content is used.
        # Check GitHub ReadMe for more info (https://github.com/louis1706/FacilityManagement/blob/main/readme.md)")]
        public Dictionary<IntercomDisplay.IcomText, string> CustomText { get; set; } = new()
        {
            { IntercomDisplay.IcomText.Ready, "Ready" },
            {
                IntercomDisplay.IcomText.Transmitting,
                "{intercom_speaker_nickname} speaking {intercom_speech_remaining_time}"
            },
            {
                IntercomDisplay.IcomText.TrasmittingBypass,
                "{intercom_speaker_nickname} speaking {intercom_speech_remaining_time}"
            },
            { IntercomDisplay.IcomText.Restarting, "Restarting please wait for {intercom_remaining_cooldown}" },
            { IntercomDisplay.IcomText.AdminUsing, "the Admin {intercom_speaker_nickname} is actually speaking" },
            { IntercomDisplay.IcomText.Muted, "Issou you are muted" },
            { IntercomDisplay.IcomText.Wait, "Wait" },
            { IntercomDisplay.IcomText.Unknown, "Unknown" },
        };

        [Description("How mush the CustomText for intercom will be refresh (empty make refresh everytick)")]
        public float? IntercomRefresh { get; set; } = null;

        [Description("If all items and ragdolls in the facility should be removed after detonation.")]
        public bool WarheadCleanup { get; set; } = true;

        [Description("Sets the config of Tesla.")]
        public TeslaBuild CustomTesla { get; set; } = new()
        {
            ActivationTime = 0.75f,
            IdleRange = 6.55f,
            TriggerRange = 5.1f,
            IgnoredRoles = new()
        };

        [Description("Sets the config of Scp914.")]
        public Scp914Build CustomScp914 { get; set; } = new()
        {
            KnobChangeCooldown = 0.15f,
            DoorCloseTime = 0.8f,
            ItemUpgradeTime = 10,
            DoorOpenTime = 13.5f,
            ActivationCooldown = 16,

        };


        [Description("Sets the health of breakable windows.")]
        public List<GlassBuild> CustomWindows { get; set; } =
        [
            new()
            {
                GlassType = GlassType.Scp079,
                Health = 5,
                DisableScpDamage = true
            },
        ];

        [Description(
            "Sets the ignored damage of breakable Door (0 will make it Destructible for everything and -1 undestructible).")]
        public List<DoorBuild> CustomDoors { get; set; } =
        [
            new DoorBuild
            {
                DoorType = DoorType.CheckpointEzHczA,
                Health = 30,
                RequiredPermission = KeycardPermissions.Checkpoints | KeycardPermissions.ScpOverride,
                RequireAllPermission = false,
                DamageTypeIgnored = DoorDamageType.Grenade | DoorDamageType.Weapon | DoorDamageType.Scp096,
            },
            
            new DoorBuild
            {
                DoorType = DoorType.CheckpointEzHczB,
                Health = 30,
                RequiredPermission = KeycardPermissions.Checkpoints | KeycardPermissions.ScpOverride,
                RequireAllPermission = false,
                DamageTypeIgnored = DoorDamageType.Grenade | DoorDamageType.Weapon | DoorDamageType.Scp096,
            },
            
            new DoorBuild
            {
                DoorType = DoorType.GR18Inner,
                Health = 150,
                RequiredPermission = KeycardPermissions.None,
                RequireAllPermission = null,
                DamageTypeIgnored = 0,
            },
        ];

        [Description("Give to x RoleType Some AHP (old name for SCP of Humeshield that are now separated so this only give AHP now).")]
        public Dictionary<RoleTypeId, AhpProccessBuild> RoleTypeHumeShield { get; set; } = new()
        {
            {
                RoleTypeId.Tutorial,
                new AhpProccessBuild{
                    Amount = 60,
                    Regen = 1.5f,
                    Efficacy = 1,
                    Sustain = 5,
                }
            },
        };
        [Description("Ability to modify the default value of the choosen ItemType.")]
        public Dictionary<ItemType, ItemBuild> CustomItem { get; set; } = new()
        {
            {
                ItemType.KeycardJanitor, new ItemBuild
                {
                    Custom = new Dictionary<string, string>()
                    {
                        { nameof(Keycard.Permissions), (KeycardPermissions.ContainmentLevelOne | KeycardPermissions.ArmoryLevelOne).ToString() },
                    }
                }
            },
            {
                ItemType.KeycardResearchCoordinator, new ItemBuild
                {
                    Custom = new Dictionary<string, string>()
                    {
                        { nameof(Keycard.Permissions), KeycardPermissions.ContainmentLevelTwo.ToString() },
                    }
                }
            },
            {
                ItemType.Coin, new ItemBuild
                {
                    Custom = new Dictionary<string, string>()
                    {
                        { nameof(Exiled.API.Features.Items.Item.Scale), new Vector3(1, 2, 1).ToString() },
                    }
                }
            },
            {
                ItemType.GrenadeHE, new ItemBuild
                {
                    Custom = new Dictionary<string, string>()
                    {
                        { nameof(ExplosiveGrenade.FuseTime), 1.ToString() },
                        { nameof(ExplosiveGrenade.Scale), (Vector3.one * 2).ToString() },
                    }
                }
            },
        };

        [Description("Ability to modify the default value of the choosen RoleTypeId.")]
        public Dictionary<RoleTypeId, RoleBuild> CustomRole { get; set; } = new()
        {
            {
                RoleTypeId.Scp3114, new RoleBuild
                {
                    Custom = new Dictionary<string, string>()
                    {
                        { nameof(Exiled.API.Features.Roles.Scp3114Role.DisguiseDuration), 600.ToString() },
                        { nameof(Exiled.API.Features.Roles.FpcRole.IsUsingStamina), false.ToString() },
                        { nameof(Exiled.API.Features.Roles.FpcRole.IsInvisible), true.ToString() },
                    },
                }
            },
        };
    }
}
