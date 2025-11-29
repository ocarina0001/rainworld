using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace RainWorld
{
    [HarmonyPatch(typeof(Pawn_GeneTracker), "CheckForOverrides", MethodType.Normal)]
    public static class Patch_Pawn_GeneTracker_CheckForOverrides
    {
        public static bool Prefix(Pawn_GeneTracker __instance)
        {
            try
            {
                List<Gene> genes = new List<Gene>(__instance.GenesListForReading);
                if (genes == null || genes.Count == 0)
                {
                    __instance.pawn.skills?.DirtyAptitudes();
                    __instance.pawn.Notify_DisabledWorkTypesChanged();
                    return false;
                }
                foreach (var g in genes)
                {
                    g.OverrideBy(null);
                }
                int n = genes.Count;
                int[] parent = new int[n];
                for (int i = 0; i < n; i++) parent[i] = i;
                int Find(int x)
                {
                    while (parent[x] != x) { parent[x] = parent[parent[x]]; x = parent[x]; }
                    return x;
                }
                void Union(int a, int b)
                {
                    int ra = Find(a);
                    int rb = Find(b);
                    if (ra != rb) parent[rb] = ra;
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (genes[i].def.ConflictsWith(genes[j].def))
                            Union(i, j);
                    }
                }
                var groups = new Dictionary<int, List<int>>();
                for (int i = 0; i < n; i++)
                {
                    int r = Find(i);
                    if (!groups.TryGetValue(r, out var list))
                    {
                        list = new List<int>();
                        groups[r] = list;
                    }
                    list.Add(i);
                }
                foreach (var kv in groups)
                {
                    var idxs = kv.Value;
                    if (idxs.Count == 1)
                        continue;
                    bool hasConflict = false;
                    for (int a = 0; a < idxs.Count && !hasConflict; a++)
                        for (int b = a + 1; b < idxs.Count; b++)
                            if (genes[idxs[a]].def.ConflictsWith(genes[idxs[b]].def))
                            {
                                hasConflict = true;
                                break;
                            }
                    if (!hasConflict)
                        continue;
                    int chosenIdxInList = Rand.RangeInclusive(0, idxs.Count - 1);
                    Gene winner = genes[idxs[chosenIdxInList]];
                    winner.OverrideBy(null);
                    foreach (int id in idxs)
                    {
                        if (genes[id] != winner)
                            genes[id].OverrideBy(winner);
                    }
                }
                __instance.pawn.skills?.DirtyAptitudes();
                __instance.pawn.Notify_DisabledWorkTypesChanged();
                return false;
            }
            catch (Exception e)
            {
                Log.Error("RAINWORLD: exception in CheckForOverrides prefix: " + e);
                return true;
            }
        }
    }
}
