using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RainWorld
{
    public class JobDriver_AnimalWeaponDrop : JobDriver
    {
        private readonly int duration = 20;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_General.Wait(duration).WithProgressBarToilDelay(TargetIndex.A);

            yield return new Toil
            {
                initAction = () =>
                {
                    var c = pawn.TryGetComp<CompEquipWeapon>();
                    if (c != null && pawn.equipment != null && pawn.equipment.HasAnything())
                    {
                        if (pawn.equipment.Contains(c.cachedWeapon))
                            pawn.equipment.TryDropEquipment(c.cachedWeapon, out _, pawn.Position);

                        c.cachedWeapon = null;
                        c.cachedWeaponId = null;
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}
