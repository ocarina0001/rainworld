using Verse;
using RimWorld;

namespace RainWorld
{
    public class Plant_BatflyGrass : Plant
    {
        private CompCanBeDormant compDormant;

        protected override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            compDormant = GetComp<CompCanBeDormant>();
            if (compDormant != null && !compDormant.Awake)
            {
                compDormant.WakeUp();

            }
        }
    }
}