using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Interfaces;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Configs;
using InventorySystem.Items;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using Scp914;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BreakableDoor = Exiled.API.Features.Doors.BreakableDoor;
using Tesla = Exiled.API.Features.TeslaGate;

namespace FacilityManagement
{
    public class EventHandlers
    {
        public EventHandlers(FacilityManagement plugin) => this.plugin = plugin;
        public FacilityManagement plugin;

        public void OnWaitingForPlayers()
        {
            var startRound = GameObject.Find("StartRound");
            if (startRound is not null || startRound.TryGetComponent<Setup>(out var setuped)) return;
            startRound.AddComponent<Setup>();
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            var startRound = GameObject.Find("StartRound");
            if (startRound is not null || startRound.TryGetComponent<Setup>(out var setuped)) return;
            startRound.AddComponent<Setup>();
        }
        
        public void OnChangingAmmo(ChangingAmmoEventArgs ev)
        {
            if (plugin.Config.InfiniteAmmo.Contains(ev.Firearm.Type))
            {
                ev.IsAllowed = false;
            }
        }
        public void OnUsingMicroHIDEnergy(UsingMicroHIDEnergyEventArgs ev) => ev.Drain *= plugin.Config.EnergyMicroHid;
        public void OnUsingRadioBattery(UsingRadioBatteryEventArgs ev) => ev.Drain *= plugin.Config.EnergyRadio;
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield is not null && plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Player.Role.Type, out AhpProccessBuild ahpProccessBuild))
            {
                ev.Player.ReferenceHub.playerStats.GetModule<AhpStat>()._activeProcesses.Clear();
                ev.Player.AddAhp(ahpProccessBuild.Amount, ahpProccessBuild.Amount, -ahpProccessBuild.Regen, ahpProccessBuild.Efficacy, ahpProccessBuild.Sustain, ahpProccessBuild.Regen > 0);
            }
            if (plugin.Config.CustomRole is not null)
                CustomRole(ev.Player);
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (plugin.Config.RoleTypeHumeShield.TryGetValue(ev.Player.Role.Type, out AhpProccessBuild ahpProccessBuild) && ahpProccessBuild.Regen > 0)
                ev.Player.ActiveArtificialHealthProcesses.First().SustainTime = ahpProccessBuild.Sustain;
        }

        public void OnDetonated()
        {
            foreach (Pickup pickup in Pickup.List.ToList())
            {
                if (pickup.Position.y < 500f)
                    pickup.Destroy();
            }
            foreach (Ragdoll ragdoll in Ragdoll.List.ToList())
            {
                if (ragdoll.Position.y < 500f)
                    ragdoll.Destroy();
            }
        }
        
        public void CustomItem(IItemEvent ev)
        {
            Item newItem = ev.Item;
            if (newItem is null || !plugin.Config.CustomItem.TryGetValue(newItem.Type, out ItemBuild itemBuild))
                return;
            foreach (KeyValuePair<string, string> e in itemBuild.Custom)
            {
                try
                {
                    PropertyInfo propertyInfo = newItem.GetType().GetProperty(e.Key);

                    if (propertyInfo != null)
                    {

                        object value = ItemBuild.Parse(e.Value, propertyInfo.PropertyType, out bool success);
                        if (success)
                        {
                            propertyInfo.SetValue(newItem, value);
                        }
                        else
                            Log.Error("invalid cast");
                    }
                    else
                        Log.Error("Property not found: " + e.Key);
                }
                catch (Exception ex)
                {
                    Log.Error($"CustomItem {newItem.Type} invalid Key '{e.Key}' or Value '{e.Value}'\n{ex}");
                }
            }
        }

        public void CustomRole(Player player)
        {
            if (player is null || !plugin.Config.CustomRole.TryGetValue(player.Role.Type, out RoleBuild roleBuild))
                return;
            foreach (KeyValuePair<string, string> e in roleBuild.Custom)
            {
                if (plugin.Config.Debug)
                    Log.Debug($"RoleType {player.Role.Type} Key '{e.Key}' or Value '{e.Value}'");
                try
                {
                    PropertyInfo propertyInfo = player.Role.GetType().GetProperty(e.Key);

                    if (propertyInfo != null)
                    {
                        if (typeof(StandardSubroutine<>).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            Log.Error($"This will be supported in an imaginary futur {e.Key}");
                            continue;
                        }
                        object value = ItemBuild.Parse(e.Value, propertyInfo.PropertyType, out bool success);
                        if (success)
                            propertyInfo.SetValue(player.Role, value);
                        else
                            Log.Error("invalid cast");
                    }
                    else
                        Log.Error("Property not found: " + e.Key);
                }
                catch (Exception ex)
                {
                    Log.Error($"CustomRole {player.Role.Type} invalid Key '{e.Key}' or Value '{e.Value}'\n{ex}");
                }
            }
        }
        
    }
}
