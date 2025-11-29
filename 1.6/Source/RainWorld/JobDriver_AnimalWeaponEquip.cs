using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RainWorld
{
    public class JobDriver_AnimalWeaponEquip : JobDriver
    {
        private const TargetIndex W = TargetIndex.A;
        private const int dur = 180;

        private ThingWithComps WeaponRef => job.GetTarget(W).Thing as ThingWithComps;

        public override bool TryMakePreToilReservations(bool f)
        {
            return pawn.Reserve(WeaponRef, job, 1, -1, null, f);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(W);
            this.FailOnBurningImmobile(W);

            yield return Toils_Goto.GotoThing(W, PathEndMode.Touch);

            yield return Toils_General.Wait(dur)
                .FailOnDestroyedNullOrForbidden(W)
                .FailOnCannotTouch(W, PathEndMode.Touch)
                .WithProgressBarToilDelay(W);

            yield return new Toil
            {
                initAction = () =>
                {
                    var c = pawn.TryGetComp<CompEquipWeapon>();
                    if (c != null)
                    {
                        if (c.cachedWeapon != null)
                            pawn.equipment.TryDropEquipment(c.cachedWeapon, out _, pawn.Position);

                        var eq = (ThingWithComps)WeaponRef.SplitOff(1);
                        pawn.equipment.AddEquipment(eq);
                        c.cachedWeaponId = eq.ThingID;
                        c.cachedWeapon = eq;
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}
