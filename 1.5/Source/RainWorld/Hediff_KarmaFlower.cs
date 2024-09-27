using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace RainWorld
{
    public class Hediff_KarmaFlower : HediffWithComps
    {
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            ResurrectionUtility.TryResurrect(pawn);
            Hediff hediffToRemove = pawn.health.hediffSet.GetFirstHediffOfDef(VariousDefOf.OCARINA_KarmaFlowerHediff);
            pawn.health.RemoveHediff(hediffToRemove);
        }
    }
}
