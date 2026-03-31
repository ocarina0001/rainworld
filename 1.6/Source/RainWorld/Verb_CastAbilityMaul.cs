using RimWorld;
using Verse;
using Verse.AI;

namespace RainWorld
{
    public class Verb_CastAbilityMaul : Verb_CastAbility
    {
        public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
        {
            Pawn caster = CasterPawn;
            Pawn target = castTarg.Pawn;
            if (caster == null || target == null || target == caster)
                return false;
            Job job = JobMaker.MakeJob(VariousDefOf.OCARINA_ArtificerMaul, target);
            caster.jobs.StartJob(job, JobCondition.InterruptForced);
            base.TryStartCastOn(castTarg, destTarg);
            return true;
        }
    }
}