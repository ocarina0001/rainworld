using RimWorld;
using Verse;
using UnityEngine;

namespace RainWorld
{
    public class Verb_CastAbilityRivuletSlide : Verb_CastAbilityJump
    {
        public override ThingDef JumpFlyerDef => DefDatabase<ThingDef>.GetNamed("RivuletSlideFlyer");

        protected override bool TryCastShot()
        {
            base.TryCastShot();
            Pawn caster = CasterPawn;
            if (caster != null && currentTarget.IsValid)
            {
                IntVec3 start = caster.Position;
                IntVec3 dest = currentTarget.Cell;
                Vector3 direction = (dest - start).ToVector3();
                if (direction.sqrMagnitude > 0.01f)
                {
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    caster.Rotation = Rot4.FromAngleFlat(angle);
                }
            }
            return true;
        }
    }
}