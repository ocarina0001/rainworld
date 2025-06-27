using UnityEngine;
using Verse;
using RimWorld;

namespace RainWorld
{
    public class CompProperties_GraphicOverlay : CompProperties
    {
        public CompProperties_GraphicOverlay()
        {
            compClass = typeof(CompGraphicOverlay);
        }
        public GraphicData graphicData;
        public bool drawOnlyAtNight;
    }

    [StaticConstructorOnStartup]
    public class CompGraphicOverlay : ThingComp
    {
        public CompProperties_GraphicOverlay Props => (CompProperties_GraphicOverlay)props;

        public override void PostDraw()
        {
            base.PostDraw();
            if (Props != null && parent.Map != null)
            {
                if (Props.drawOnlyAtNight == true)
                {
                    if (GenLocalDate.HourInteger(parent.Map) >= 21 || GenLocalDate.HourInteger(parent.Map) < 6)
                    {
                        Vector3 drawPos = parent.DrawPos + Props.graphicData.drawOffset;
                        Props.graphicData.Graphic.color = parent.Graphic.color;
                        Props.graphicData.Graphic.Draw(drawPos, parent.Rotation, parent);
                    }
                }
                else
                {
                    Vector3 drawPos = parent.DrawPos + Props.graphicData.drawOffset;
                    Props.graphicData.Graphic.Draw(drawPos, parent.Rotation, parent);
                }
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
        }
    }
}
