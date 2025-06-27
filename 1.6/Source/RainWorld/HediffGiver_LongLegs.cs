using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RainWorld
{
    class HediffGiver_LongLegs : HediffGiver
    {
        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            base.OnIntervalPassed(pawn, cause);
            if (!pawn.health.hediffSet.HasHediff(VariousDefOf.OCARINA_LongLegsHediff))
                pawn.health.AddHediff(VariousDefOf.OCARINA_LongLegsHediff);
        }
    }
}
