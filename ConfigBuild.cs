using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using Exiled.API.Extensions;
using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Scp914;

namespace FacilityManagement
{
    public class AhpProccessBuild
    {
        public float Amount { get; set; }
        public float Regen { get; set; }
        public float Efficacy { get; set; }
        public float Sustain { get; set; }
    }

    public class DoorBuild
    {
        public DoorType DoorType { get; set; }
        public float? Health { get; set; }
        public KeycardPermissions? RequiredPermission { get; set; }
        public bool? RequireAllPermission { get; set; }
        public DoorDamageType? DamageTypeIgnored { get; set; }

        public void Setup()
        {
            foreach (var door in Door.List)
            {
                if (door.Type != DoorType) continue;
                
                BreakableDoor breakabledoor = door.As<BreakableDoor>();

                if (FacilityManagement.Instance.Config.Debug)
                {
                    string Debug = $"[CustomDoor] : {DoorType}\n";
                    Debug += $"Health: {(breakabledoor is null ? "Nan" : breakabledoor.Health)} => {Health.Value}\n";
                    Debug += $"IgnoredDamageTypes: {(breakabledoor is null ? "Nan" : breakabledoor.IgnoredDamage)} => {DamageTypeIgnored}\n";
                    Debug += $"RequiredPermissions: {door.KeycardPermissions} => {RequiredPermission}\n";
                    Debug += $"RequireAllPermission: {door.Base.RequiredPermissions.RequireAll} => {RequireAllPermission}\n";
                    Log.Debug(Debug);
                }

                if (Health is not null && breakabledoor is not null)
                    breakabledoor.Health = Health.Value;
                if (DamageTypeIgnored is not null && breakabledoor is not null)
                    breakabledoor.IgnoredDamage = DamageTypeIgnored.Value;
                if (RequiredPermission is not null)
                    door.KeycardPermissions = RequiredPermission.Value;
                if (RequireAllPermission is not null)
                    door.Base.RequiredPermissions.RequireAll = RequireAllPermission.Value;
            }
        }
    }

    public class GlassBuild
    {
        public GlassType? GlassType { get; set; }
        
        public float? Health { get; set; }
        public bool? DisableScpDamage { get; set; }

        public void Setup()
        {
            string debug = "[CustomWindow]\n";
                
            debug += $"Type: {GlassType}\n";
            debug += $"Health: {Health} => {Health}\n";
            debug += $"DisableScpDamage: {DisableScpDamage} => {DisableScpDamage}\n\n";

            if (Health is not null)
                Health = Health.Value;
            if (DisableScpDamage is not null)
                DisableScpDamage = DisableScpDamage.Value;
                
            Log.Debug(debug);
            
            foreach (Window window in Window.List)
            {
                if (GlassType != window.Type) continue;
                
                if (Health is not null)
                    window.Health = Health.Value;
                if (DisableScpDamage is not null)
                    window.DisableScpDamage = DisableScpDamage.Value;
            }
        }
    }

    public class TeslaBuild
    {
        public List<RoleTypeId> IgnoredRoles { get; set; }
        public float? TriggerRange { get; set; }
        public float? IdleRange { get; set; }
        public float? ActivationTime { get; set; }
        public float? CooldownTime { get; set; }

        public void Setup()
        {
            if (FacilityManagement.Instance.Config.Debug)
            {
                string Debug = "[CustomTesla]\n";
                {
                    Exiled.API.Features.TeslaGate tesla = Exiled.API.Features.TeslaGate.List.First();
                    Debug += $"CooldownTime: {tesla.CooldownTime} => {CooldownTime}\n";
                    Debug += $"IdleRange: {tesla.IdleRange} => {IdleRange}\n\n";
                    Debug += $"TriggerRange: {tesla.TriggerRange} => {TriggerRange}\n\n";
                    Debug += $"ActivationTime: {tesla.ActivationTime} => {ActivationTime}\n\n";
                    Debug += $"IgnoredRoles: {string.Join(",", Exiled.API.Features.TeslaGate.IgnoredRoles)} => {(IgnoredRoles is null ? "Null" : string.Join(",", IgnoredRoles))}\n\n";
                }
                Log.Debug(Debug);
            }

            foreach (Exiled.API.Features.TeslaGate tesla in Exiled.API.Features.TeslaGate.List)
            {
                if(CooldownTime.HasValue)
                    tesla.CooldownTime = CooldownTime.Value;
                
                if(IdleRange.HasValue)
                    tesla.IdleRange = IdleRange.Value;
                
                if(TriggerRange.HasValue)
                    tesla.TriggerRange = TriggerRange.Value;
                
                if(ActivationTime.HasValue)
                    tesla.ActivationTime = ActivationTime.Value;
            }

            Exiled.API.Features.TeslaGate.IgnoredRoles = IgnoredRoles;
                
        }
    }

    public class Scp914Build
    {
        public float? KnobChangeCooldown { get; set; }
        public float? DoorCloseTime { get; set; }
        public float? ItemUpgradeTime { get; set; }
        public float? DoorOpenTime { get; set; }
        public float? ActivationCooldown { get; set; }

        public void Setup()
        {
            Scp914Controller scp914 = Exiled.API.Features.Scp914.Scp914Controller;
            string debug = "[Custom914]\n";
            {
                debug += $"KnobChangeCooldown: {scp914.KnobChangeCooldown} => {KnobChangeCooldown}\n";
                debug += $"DoorOpenTime: {scp914.DoorOpenTime} => {DoorOpenTime}\n";
                debug += $"ItemUpgradeTime: {scp914.ItemUpgradeTime} => {ItemUpgradeTime}\n";
                debug += $"DoorCloseTime: {scp914.DoorCloseTime} => {DoorCloseTime}\n";
                debug += $"TotalSequenceTime: {scp914.TotalSequenceTime} => {ActivationCooldown}\n";
            }
            Log.Debug(debug);

            if (KnobChangeCooldown is not null)
                scp914.KnobChangeCooldown = KnobChangeCooldown.Value;
            
            if (DoorOpenTime is not null)
                scp914.DoorOpenTime = DoorOpenTime.Value;
            
            if (ItemUpgradeTime is not null)
                scp914.ItemUpgradeTime = ItemUpgradeTime.Value;
            
            if (DoorCloseTime is not null)
                scp914.DoorCloseTime = DoorCloseTime.Value;
            
            if (ActivationCooldown is not null)
                scp914.TotalSequenceTime = ActivationCooldown.Value;
        }
    }

    public class GeneratorBuild
    {
        public float? UnlockCooldown { get; set; }
        public float? LeverDelay { get; set; }
        public float? DoorPanelCooldown { get; set; }
        public float? InteractionCooldown { get; set; }
        public float? DeactivationTime { get; set; }
        public KeycardPermissions? RequiredPermission { get; set; }

        public void Setup()
        {
            if (FacilityManagement.Instance.Config.Debug)
            {
                Generator generator = Generator.List.First();
                string Debug = "[CustomGenerator]\n";
                Debug += $"UnlockCooldown: {generator.UnlockCooldown} => {UnlockCooldown}\n";
                Debug += $"LeverDelay: {generator.LeverDelay} => {LeverDelay}\n";
                Debug += $"TogglePanelCooldown: {generator.TogglePanelCooldown} => {DoorPanelCooldown}\n";
                Debug += $"InteractionCooldown: {generator.InteractionCooldown} => {InteractionCooldown}\n";
                Debug += $"DeactivationTime: {generator.DeactivationTime} => {DeactivationTime}\n";
                Debug += $"KeycardPermissions: {generator.KeycardPermissions} => {RequiredPermission}\n";
                Log.Debug(Debug);
            }
            foreach (Generator generator in Generator.List)
            {
                if (UnlockCooldown is not null)
                    generator.UnlockCooldown = UnlockCooldown.Value;
                if (RequiredPermission is not null)
                    generator.KeycardPermissions = RequiredPermission.Value;
                if (LeverDelay is not null)
                    generator.LeverDelay = LeverDelay.Value;
                if (DoorPanelCooldown is not null)
                    generator.TogglePanelCooldown = DoorPanelCooldown.Value;
                if (InteractionCooldown is not null)
                    generator.InteractionCooldown = InteractionCooldown.Value;
                if (DeactivationTime is not null)
                    generator.DeactivationTime = DeactivationTime.Value;
            }
        }
    }

    public class ItemBuild
    {
        public Dictionary<string, string> Custom { get; set; }

        public static object Parse(string value, Type targetType, out bool success)
        {
            success = true;

            // Handle enums
            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, value);
            }

            // Handle Vector3 (assuming the format is "(x,y)" "(x,y,z)""(x,y,z,w)")
            if (targetType == typeof(Vector2) || targetType == typeof(Vector3) || targetType == typeof(Vector4))
            {
                success = true;

                value = value.Replace('(', ' ').Replace(')', ' ').Replace(" ", string.Empty);
                string[] components = value.Split(',');
                float x, y, z, w;

                if (components.Length == 2 && float.TryParse(components[0], out x) && float.TryParse(components[1], out y))
                {
                    return new Vector2(x, y);
                }
                if (components.Length == 3 && float.TryParse(components[0], out x) && float.TryParse(components[1], out y) && float.TryParse(components[2], out z))
                {
                    return new Vector3(x, y, z);
                }
                if (components.Length == 2 && float.TryParse(components[0], out x) && float.TryParse(components[1], out y) && float.TryParse(components[2], out z) && float.TryParse(components[2], out w))
                {
                    return new Vector4(x, y, z, w);
                }
                success = false;

                Log.Info($"Invalid Vector {value}");
                return default;
            }

            // Handle nullable value types (e.g., int?, float?, double?)
            if (Nullable.GetUnderlyingType(targetType) != null || targetType.IsValueType)
            {
                // Check if the value is null or empty and return null
                if (string.IsNullOrWhiteSpace(value))
                {
                    success = false;
                    return default;
                }

                try
                {
                    return Convert.ChangeType(value, targetType);
                }
                catch (InvalidCastException)
                {
                    success = false;
                    return default;
                }
            }

            // WTF are you really implemented a AnimationCurve config are you fine bro ???
            if (targetType == typeof(AnimationCurve))
            {
                return default;
            }

            Log.Error("End");
            success = false;
            return default;
        }
    }
    public class RoleBuild
    {
        public Dictionary<string, string> Custom { get; set; }
    }
    public class AnnimationCurveBuild
    {
        public float? AddCurve { get; set; }
        public float? MultiplyCurve { get; set; }

        public AnimationCurve ModifyCurve(AnimationCurve animationCurve)
        {
            if (AddCurve is not null)
                animationCurve.Add(AddCurve.Value);
            if (MultiplyCurve is not null)
                animationCurve.Multiply(MultiplyCurve.Value);
            return animationCurve;
        }
    }
    /* Wait Futur Exiled

public class FlashbangBuild
{
    public AnnimationCurveBuild BlindingOverDistance { get; set; }
    public AnnimationCurveBuild TurnedAwayBlindingDistance { get; set; }
    public AnnimationCurveBuild DeafenDurationOverDistance { get; set; }
    public AnnimationCurveBuild TurnedAwayDeafenDurationOverDistance { get; set; }
    public float? DurfaceZoneDistanceIntensifier { get; set; }
    public float? AdditionalBlurDuration { get; set; }
    public float? MinimalEffectDuration { get; set; }
    public float? BlindTime { get; set; }
}

public class FragGrenadeBuild
{
    public AnnimationCurveBuild BlindingOverDistance { get; set; }
    public AnnimationCurveBuild TurnedAwayBlindingDistance { get; set; }
    public AnnimationCurveBuild DeafenDurationOverDistance { get; set; }
    public AnnimationCurveBuild TurnedAwayDeafenDurationOverDistance { get; set; }
    public float? DurfaceZoneDistanceIntensifier { get; set; }
    public float? AdditionalBlurDuration { get; set; }
    public float? MinimalEffectDuration { get; set; }
    public float? BlindTime { get; set; }
}

public class Scp244Build
{
    public AnnimationCurveBuild DamageOverTemperature { get; set; }
    public AnnimationCurveBuild GrowSpeedOverLifetime { get; set; }
    public float? MaxExitTemp { get; set; }
    public float? TemperatureDrop { get; set; }
    public float? MinimalEffectDuration { get; set; }
    public float? BlindTime { get; set; }
    public float? Scp244Health { get; set; }
    public float? DeployedPickupTime { get; set; }
}
/*
public class Scp018Build
{
    public AnnimationCurveBuild DamageOverVelocity { get; set; }
    public float? MaximumVelocity { get; set; }
    public float? OnBounceVelocityAddition { get; set; }
    public float? ActivationVelocitySqr { get; set; }
    public float? DoorDamageMultiplier { get; set; }
    public float? ScpDamageMultiplier { get; set; }
    public float? FriendlyFireTime { get; set; }
    public float? BounceHitregRadius { get; set; }
}

public class ExplosionGrenadeBuild
{
    public AnnimationCurveBuild PlayerDamageOverDistance { get; set; }
    public AnnimationCurveBuild EffectDurationOverDistance { get; set; }
    public AnnimationCurveBuild DoorDamageOverDistance { get; set; }
    public float? ScpDamageMultiplier { get; set; }
    public float? MaxRadius { get; set; }
    public float? BurnedDuration { get; set; }
    public float? DeafenedDuration { get; set; }
    public float? ConcussedDuration { get; set; }
    public float? MinimalDuration { get; set; }
    public float? BounceHitregRadius { get; set; }
}

public class RegenerationBuild
{
    public AnnimationCurveBuild Scp500HealProgress { get; set; }
    public AnnimationCurveBuild PainkillersHealProgress { get; set; }
}
public class Scp939Build
{
    public AnnimationCurveBuild StaminaRegeneration { get; set; }
}*/
}
