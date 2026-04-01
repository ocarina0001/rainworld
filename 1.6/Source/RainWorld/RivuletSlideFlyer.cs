using RimWorld;
using Verse;
using UnityEngine;

namespace RainWorld
{
    public class RivuletSlideFlyer : PawnFlyer
    {
        private bool rotationSet = false;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!rotationSet && FlyingPawn != null)
            {
                Vector3 direction = DestinationPos - FlyingPawn.DrawPos;
                if (direction.sqrMagnitude > 0.01f)
                {
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    FlyingPawn.Rotation = Rot4.FromAngleFlat(angle);
                }
                rotationSet = true;
            }
        }
    }
}