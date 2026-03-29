using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;

namespace RainWorld
{
    public class Verb_CastAbilityExplosiveJump : Verb_CastAbilityJump
    {
        private float ExplosionRadiusPerDistance
        {
            get
            {
                var comp = ability?.comps?.OfType<CompAbility_ExplosiveJump>().FirstOrDefault();
                if (comp != null && comp.Props.radiusPerDistance > 0f)
                    return comp.Props.radiusPerDistance;
                return 0.5f;
            }
        }

        protected override bool TryCastShot()
        {
            Map map = CasterPawn.Map;
            IntVec3 startPos = CasterPawn.Position;
            LocalTargetInfo target = currentTarget;
            bool jumpStarted = base.TryCastShot();
            if (jumpStarted && map != null)
            {
                float distance = startPos.DistanceTo(target.Cell);
                float radius = distance * ExplosionRadiusPerDistance;
                radius = Mathf.Max(0.5f, radius);
                GenExplosion.DoExplosion(startPos, map, radius, DamageDefOf.Bomb, CasterPawn);
            }
            return jumpStarted;
        }
    }

    public class CompProperties_AbilityExplosiveJump : CompProperties_AbilityEffect
    {
        public float radiusPerDistance = 0.5f;
        public CompProperties_AbilityExplosiveJump()
        {
            compClass = typeof(CompAbility_ExplosiveJump);
        }
    }

    public class CompAbility_ExplosiveJump : CompAbilityEffect
    {
        public new CompProperties_AbilityExplosiveJump Props => (CompProperties_AbilityExplosiveJump)props;
    }
}