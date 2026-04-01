using RimWorld;
using Verse;

namespace RainWorld
{
    public class PawnFlyerWorker_StraightLine : PawnFlyerWorker
    {
        public PawnFlyerWorker_StraightLine(PawnFlyerProperties properties) : base(properties) { }
        public override float AdjustedProgress(float progress) => progress;
        public override float GetHeight(float progress) => 0f;
    }
}