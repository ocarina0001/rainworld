using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RainWorld
{
    public class PawnRenderNode_Eyes : PawnRenderNode_AttachmentHead
    {
        private Color? cachedColor;
        private static readonly System.Random rng = new System.Random();

        public PawnRenderNode_Eyes(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree) { }
        public override Color ColorFor(Pawn pawn)
        {
            if (cachedColor != null) return cachedColor.Value;
            if (rng.Next(50) == 0)
            {
                cachedColor = new Color(
                    (float)rng.NextDouble(),
                    (float)rng.NextDouble(),
                    (float)rng.NextDouble());
                return cachedColor.Value;
            }
            var gene = pawn.genes?.GenesListForReading?.FirstOrDefault(g => g.def.renderNodeProperties?.Any(p => p.nodeClass == GetType()) == true);
            var ext = gene?.def?.GetModExtension<DefModExtension_ColorsList>();
            if (ext != null && ext.colors?.Count > 0)
                cachedColor = ext.colors[pawn.thingIDNumber % ext.colors.Count];
            else
                cachedColor = Color.red;
            return cachedColor.Value;
        }
    }
}
