using Verse;
using RimWorld;

namespace RainWorld
{
    public class CompProperties_TextureSwapConditional : CompProperties
    {
        public class DrawBetweenHour
        {
            public int hourStart;
            public int hourEnd;
        }

        public CompProperties_TextureSwapConditional()
        {
            compClass = typeof(CompTextureSwapConditional);
        }

        public DrawBetweenHour drawBetweenHour;
        public GraphicData originalGraphicData;
    }

    [StaticConstructorOnStartup]
    public class CompTextureSwapConditional : ThingComp
    {
        public CompProperties_TextureSwapConditional Props => (CompProperties_TextureSwapConditional)props;
        public Pawn pawn => parent as Pawn;
        public bool isTime;
        public bool wasTime;

        public override void CompTick()
        {
            base.CompTick();

            if (pawn.Map != null && Props != null && Props.drawBetweenHour != null)
            {
                int currentHour = GenLocalDate.HourInteger(pawn.Map);
                int start = Props.drawBetweenHour.hourStart;
                int end = Props.drawBetweenHour.hourEnd;

                if (start < end)
                {
                    isTime = currentHour >= start && currentHour < end;
                }
                else
                {
                    isTime = currentHour >= start || currentHour < end;
                }

                if (isTime != wasTime)
                {
                    ForceRedraw(pawn);
                    wasTime = isTime;
                }
            }
        }

        public static void ForceRedraw(Pawn pawn)
        {
            pawn.Drawer?.renderer.SetAllGraphicsDirty();
        }
    }
}
