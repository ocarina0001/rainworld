using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace RainWorld
{
    public class Verb_CastAbilityExplosiveCraft : Verb_CastAbility
    {
        protected override bool TryCastShot()
        {
            base.TryCastShot();
            Pawn caster = CasterPawn;
            if (caster == null || !caster.Spawned)
                return false;
            var comp = ability?.comps?.OfType<CompAbility_ExplosiveCraft>().FirstOrDefault();
            if (comp == null || comp.Props.spawnableItems.NullOrEmpty())
                return false;
            int count = Rand.Range(1, 10);
            for (int i = 0; i < count; i++)
            {
                ThingDef itemDef = comp.Props.spawnableItems.RandomElement();
                if (itemDef != null)
                {
                    Thing item = ThingMaker.MakeThing(itemDef);
                    GenSpawn.Spawn(item, caster.Position, caster.Map);
                }
            }
            return true;
        }
    }

    public class CompProperties_ExplosiveCraft : CompProperties_AbilityEffect
    {
        public List<ThingDef> spawnableItems;
        public CompProperties_ExplosiveCraft()
        {
            compClass = typeof(CompAbility_ExplosiveCraft);
        }
    }

    public class CompAbility_ExplosiveCraft : CompAbilityEffect
    {
        public new CompProperties_ExplosiveCraft Props => (CompProperties_ExplosiveCraft)props;
    }
}