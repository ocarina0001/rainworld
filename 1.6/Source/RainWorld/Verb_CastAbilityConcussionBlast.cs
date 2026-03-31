using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RainWorld
{
    public class Verb_CastAbilityConcussionBlast : Verb_CastAbility
    {
        protected override bool TryCastShot()
        {
            base.TryCastShot();
            Pawn caster = CasterPawn;
            Map map = caster.Map;
            if (map == null) return false;
            float explosionRadius = 3.5f;
            int damageAmount = 2;
            GenExplosion.DoExplosion(caster.Position, map, explosionRadius, DamageDefOf.Bomb, caster, damageAmount);
            List<Pawn> targets = new List<Pawn>();
            foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
            {
                if (thing is Pawn pawn && pawn != caster && pawn.Position.DistanceTo(caster.Position) <= explosionRadius)
                    targets.Add(pawn);
            }
            foreach (Pawn target in targets)
            {
                float distance = target.Position.DistanceTo(caster.Position);
                float distanceFactor = Mathf.Clamp01(1f - (distance / explosionRadius));
                if (Rand.Chance(0.8f))
                {
                    int stunTicks = Mathf.RoundToInt(60 * (1 + distanceFactor * 4));
                    target.stances.stunner.StunFor(stunTicks, caster);
                }
                if (Rand.Chance(0.4f))
                {
                    if (target.equipment != null && target.equipment.Primary != null)
                        target.equipment.DropAllEquipment(target.Position, true);
                }
            }
            return true;
        }
    }
}