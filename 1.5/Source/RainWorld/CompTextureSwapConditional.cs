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
        //public bool isTime;
        //public bool wasTime;
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
            //Log.Message("isTime: " + isTime + " | wasTime: " + wasTime);
            if (pawn.Map != null && Props != null && Props.drawBetweenHour != null)
            {
                if (GenLocalDate.HourInteger(pawn.Map) >= Props.drawBetweenHour.hourEnd || GenLocalDate.HourInteger(pawn.Map) <= Props.drawBetweenHour.hourStart)
                {
                    isTime = true;
                }
                else 
                {
                    isTime = false;
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