using RimWorld;
using Verse;

namespace RainWorld
{
    public class Verb_CastAbilityRainSummon : Verb_CastAbility
    {
        protected override bool TryCastShot()
        {
            Pawn caster = CasterPawn;
            if (caster == null || !caster.Spawned)
                return false;
            Map map = caster.Map;
            if (map == null)
                return false;
            map.weatherManager.TransitionTo(VariousDefOf.Rain);
            return base.TryCastShot();
        }
    }
}