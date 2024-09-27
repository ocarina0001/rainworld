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
        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();
            ResurrectionUtility.Resurrect(pawn);
            Hediff hediffToRemove = pawn.health.hediffSet.GetFirstHediffOfDef(VariousDefOf.OCARINA_KarmaFlowerHediff);
            pawn.health.RemoveHediff(hediffToRemove);
        }
    }
}
