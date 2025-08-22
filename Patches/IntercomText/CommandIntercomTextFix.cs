﻿using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.MutingAndIntercom;
using Exiled.API.Features;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.Voice;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using static HarmonyLib.AccessTools;

namespace FacilityManagement.Patches.IntercomText
{
    [HarmonyPatch(typeof(IntercomDisplay), nameof(IntercomDisplay.TrySetDisplay))]
    public static class IntercomTextCommandFix
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            // Facility Management Fix
            int index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Ldsfld);
            newInstructions[index].operand = Field(typeof(FacilityManagement), nameof(FacilityManagement.Instance));

            newInstructions.RemoveRange(++index, 2);
            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new(OpCodes.Ldarg_0),
                new(OpCodes.Stfld, Field(typeof(FacilityManagement),nameof(FacilityManagement.Instance.CustomText))),
            });

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}