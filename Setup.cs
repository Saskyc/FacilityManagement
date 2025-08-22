using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace FacilityManagement
{
    public class Setup : MonoBehaviour
    {
        public void Awake()
        {
            FacilityManagement.Instance.Config.CustomTesla?.Setup();
            FacilityManagement.Instance.Config.CustomScp914?.Setup();

            foreach (var customWindow in FacilityManagement.Instance.Config.CustomWindows)
                customWindow?.Setup();

            foreach (var door in FacilityManagement.Instance.Config.CustomDoors)
                door?.Setup();
            
            FacilityManagement.Instance.Config.CustomGenerator?.Setup();
            
            if (FacilityManagement.Instance.Config.LiftMoveDuration is not null)
            {
                foreach (var elevator in Lift.List)
                    if (FacilityManagement.Instance.Config.LiftMoveDuration.TryGetValue(elevator.Type, out float value))
                        elevator.AnimationTime = value;
            }
        }
    }
}