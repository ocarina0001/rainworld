using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RainWorld
{
    public class DefModExtension_ColorsList : DefModExtension
    {
        public List<string> colorStrings;
        public List<Color> colors;

        public override void ResolveReferences(Def parentDef)
        {
            //Log.Message("RAINWORLD: ResolveReferences called for " + parentDef?.defName);
            base.ResolveReferences(parentDef);
            if (colorStrings == null || colorStrings.Count == 0)
            {
                //Log.Message("RAINWORLD: No colorStrings found.");
                colors = null;
                return;
            }
           // Log.Message("RAINWORLD: Parsing " + colorStrings.Count + " colorStrings.");
            colors = new List<Color>(colorStrings.Count);
            var regex = new Regex(@"\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)");
            foreach (var s in colorStrings)
            {
                //Log.Message("RAINWORLD: Raw string -> " + s);
                if (string.IsNullOrWhiteSpace(s))
                {
                    //Log.Message("RAINWORLD: Skipping empty string.");
                    continue;
                }
                var m = regex.Match(s.Trim());
                if (!m.Success)
                {
                    //Log.Message("RAINWORLD: Regex failed for string: " + s);
                    continue;
                }
                if (float.TryParse(m.Groups[1].Value, out float r) &&
                    float.TryParse(m.Groups[2].Value, out float g) &&
                    float.TryParse(m.Groups[3].Value, out float b))
                {
                    Color c = new Color(r / 255f, g / 255f, b / 255f);
                    colors.Add(c);
                    //Log.Message($"RAINWORLD: Parsed color ({r}, {g}, {b}) -> {c}");
                }
                //else
                //{
                //    Log.Message("RAINWORLD: Failed to parse numeric values for: " + s);
                //}
            }
            if (colors.Count == 0)
            {
                //Log.Message("RAINWORLD: No valid colors parsed.");
                colors = null;
            }
            //else
            //{
            //    Log.Message("RAINWORLD: Parsed " + colors.Count + " valid colors.");
            //}
        }
    }
}
