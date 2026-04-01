using RimWorld;
using Verse;

namespace RainWorld
{
    public class HediffGiver_WholeBody : HediffGiver
    {
        public override void OnIntervalPassed(Pawn pawn, Hediff hediff)
        {
            if (!pawn.health.hediffSet.HasHediff(this.hediff))
                pawn.health.AddHediff(this.hediff, null, null);
        }
    }
}