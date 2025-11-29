using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RainWorld
{
    internal class Patch_AnimalWeaponGizmos
    {
        [HarmonyPatch(typeof(Pawn))]
        [HarmonyPatch("GetGizmos")]
        internal class Patch_Pawn_GetGizmo
        {
            private static void Postfix(ref IEnumerable<Gizmo> __result, Pawn __instance)
            {
                if (__result == null) return;
                var list = __result.ToList();
                if (__instance.Faction != null && __instance.Faction == Faction.OfPlayer && __instance.RaceProps.Animal)
                {
                    var c = __instance.TryGetComp<CompEquipWeapon>();
                    if (c != null)
                    {
                        if (c.cachedWeapon != null)
                        {
                            Texture2D tex = c.cachedWeapon.def.graphicData == null ? ContentFinder<Texture2D>.Get("UI/Icons/Animal/Tame") : ContentFinder<Texture2D>.Get(c.cachedWeapon.def.graphicData.texPath);

                            var cmd = new Command_Action
                            {
                                defaultLabel = "drop",
                                defaultDesc = "drop weapon.",
                                icon = tex,
                                hotKey = KeyBindingDefOf.Misc12,
                                action = () =>
                                {
                                    var j = JobMaker.MakeJob(VariousDefOf.OCARINA_RainWorldDropWeapon);
                                    __instance.jobs.TryTakeOrderedJob(j, JobTag.Misc);
                                }
                            };
                            list.Add(cmd);
                        }
                        else
                        {
                            var cmd = new Command_Target
                            {
                                defaultLabel = "equip",
                                defaultDesc = "equip weapon.",
                                icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Tame"),
                                hotKey = KeyBindingDefOf.Misc12,
                                targetingParams = new TargetingParameters
                                {
                                    canTargetPawns = false,
                                    canTargetBuildings = false,
                                    canTargetItems = true,
                                    mapObjectTargetsMustBeAutoAttackable = false,
                                    thingCategory = ThingCategory.Item,
                                    validator = t => t.Thing is ThingWithComps twc && (twc.def.IsMeleeWeapon || twc.def.IsRangedWeapon)
                                },
                                action = target =>
                                {
                                    var j = JobMaker.MakeJob(VariousDefOf.OCARINA_RainWorldEquipWeapon, target);
                                    __instance.jobs.TryTakeOrderedJob(j, JobTag.Misc);
                                }
                            };
                            list.Add(cmd);
                        }
                    }
                }
                __result = list;
            }
        }
    }
}
