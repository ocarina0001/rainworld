using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RainWorld
{
    [HarmonyPatch(typeof(Pawn), "ButcherProducts")]
    public static class Patch_ButcherProducts
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> __result, Pawn __instance)
        {
            foreach (var thing in __result)
            {
                if (thing.def.IsStuff && thing.def.stuffProps?.categories?.Contains(StuffCategoryDefOf.Leathery) == true)
                {
                    var ext = __instance.def.GetModExtension<DefModExtension_LeatherColor>();
                    if (ext != null)
                        thing.SetColor(ext.color);
                }
                yield return thing;
            }
        }
    }
}
