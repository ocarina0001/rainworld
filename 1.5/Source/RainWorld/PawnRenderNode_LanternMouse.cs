using Verse;

namespace RainWorld
{
    public class PawnRenderNode_LanternMouse : PawnRenderNode_AnimalPart
    {
        public PawnRenderNode_LanternMouse(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree) { }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (pawn.TryGetComp<CompTextureSwapConditional>(out var comp) && comp != null && comp.Props != null && comp.isTime && tree.Resolved)
            {
                //Log.Message("It is time to change texture for " + pawn);
                Graphic graphic = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
                return GraphicDatabase.Get<Graphic_Multi>(graphic.path + "_night", ShaderDatabase.CutoutComplex, graphic.drawSize, graphic.color);
            }
            else
            {
                //Log.Message(comp.Props.isTime);
                return base.GraphicFor(pawn);
            }
        }
    }
}
