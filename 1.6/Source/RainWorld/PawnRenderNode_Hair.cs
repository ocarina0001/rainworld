using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RainWorld
{
    public class PawnRenderNode_Hair : PawnRenderNode
    {
        private Color? cachedColor;
        private static readonly System.Random rng = new System.Random();

        public PawnRenderNode_Hair(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree) { }

        public override Color ColorFor(Pawn pawn)
        {
            //Log.Message("RAINWORLD: ColorFor called for pawn " + pawn?.Name?.ToStringShort);
            if (cachedColor != null)
            {
                //Log.Message("RAINWORLD: Returning cached color " + cachedColor.Value);
                return cachedColor.Value;
            }
            if (rng.Next(50) == 0)
            {
                cachedColor = new Color(
                    (float)rng.NextDouble(),
                    (float)rng.NextDouble(),
                    (float)rng.NextDouble());
                //Log.Message("RAINWORLD: Random override color chosen: " + cachedColor.Value);
                return cachedColor.Value;
            }
            //Log.Message("RAINWORLD: Searching for gene with matching render node type...");
            var gene = pawn.genes?.GenesListForReading?.FirstOrDefault(g => g.def.renderNodeProperties?.Any(p => p.nodeClass == GetType()) == true);
            if (gene == null)
            {
                //Log.Message("RAINWORLD: No gene found matching this render node.");
            }
            else
            {
                //Log.Message("RAINWORLD: Found gene: " + gene.def.defName);
            }
            var ext = gene?.def?.GetModExtension<DefModExtension_ColorsList>();
            if (ext == null)
            {
                //Log.Message("RAINWORLD: No DefModExtension_ColorsList found on gene.");
            }
            //else if (ext.colors == null || ext.colors.Count == 0)
            //{
            //    Log.Message("RAINWORLD: DefModExtension_ColorsList found but has no colors.");
            //}
            else
            {
                int index = pawn.thingIDNumber % ext.colors.Count;
                cachedColor = ext.colors[index];
                //Log.Message($"RAINWORLD: Using color index {index}/{ext.colors.Count} -> {cachedColor.Value}");
                return cachedColor.Value;
            }
            cachedColor = pawn.story?.HairColor ?? Color.white;
            //Log.Message("RAINWORLD: Fallback color used -> " + cachedColor.Value);
            return cachedColor.Value;
        }
    }
}
