using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace RainWorld
{
    public class JobDriver_AnimalWeaponShoot : JobDriver
    {
        private const TargetIndex T = TargetIndex.A;
        private IAttackTarget TargetRef => job.GetTarget(T).Thing as IAttackTarget;

        public override bool TryMakePreToilReservations(bool f)
        {
            if (TargetRef == null) return false;
            if (pawn == null || pawn.Map == null) return false;
            if (pawn.TryGetComp<CompEquipWeapon>() == null) return false;
            pawn.Map.attackTargetReservationManager.Reserve(pawn, job, TargetRef);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return new Toil
            {
                initAction = () =>
                {
                    var c = pawn.TryGetComp<CompEquipWeapon>();
                    if (c != null && !c.rangedSet.NullOrEmpty())
                    {
                        var v = c.rangedSet.RandomElement();
                        v.caster = pawn;
                        v.castCompleteCallback = () =>
                        {
                            if (TargetA.Thing is Pawn tp && !tp.Dead && !tp.Downed)
                            {
                                var j = JobMaker.MakeJob(JobDefOf.AttackMelee, TargetA);
                                pawn.jobs.TryTakeOrderedJob(j, JobTag.Misc);
                            }
                        };
                        pawn.CurJob.verbToUse = v;
                        bool fired = v.TryStartCastOn(TargetA.Thing);
                    }
                },
                tickAction = () =>
                {
                    var tg = TargetA.Thing;
                    var v = pawn.CurJob.verbToUse;
                    if (tg == null)
                    {
                        v.state = VerbState.Idle;
                        pawn.jobs.StopAll();
                    }
                    else if (!GenSight.LineOfSightToEdges(pawn.Position, tg.Position, pawn.Map))
                    {
                        v.state = VerbState.Idle;
                        pawn.jobs.StopAll();
                    }
                    else if (tg is Pawn tp && !tp.Dead && !tp.Downed)
                    {
                        float dist = (pawn.Position - tg.Position).LengthHorizontal;
                        if (dist < 2f)
                        {
                            v.state = VerbState.Idle;
                            pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AttackMelee, TargetA), JobTag.Misc);
                        }
                        else if (dist > v.verbProps.range)
                        {
                            v.state = VerbState.Idle;
                            pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AttackMelee, TargetA), JobTag.Misc);
                        }
                        else
                            v.VerbTick();
                    }
                    else
                    {
                        v.state = VerbState.Idle;
                        pawn.jobs.StopAll();
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
        }
    }
}
