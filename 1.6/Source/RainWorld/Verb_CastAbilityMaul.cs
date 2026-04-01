using RimWorld;
using Verse;
using Verse.AI;

namespace RainWorld
{
    public class Verb_CastAbilityMaul : Verb_CastAbility
    {
        protected override bool TryCastShot()
        {
            Pawn caster = CasterPawn;
            Pawn target = currentTarget.Pawn;
            if (caster == null || target == null || target == caster)
                return false;
            Job job = JobMaker.MakeJob(VariousDefOf.OCARINA_ArtificerMaul, target);
            caster.jobs.StartJob(job, JobCondition.InterruptForced);
            ability.StartCooldown(2500);
            return true;
        }
    }
}