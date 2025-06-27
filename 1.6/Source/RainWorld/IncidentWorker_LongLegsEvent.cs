using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;

namespace RainWorld
{
    public class IncidentWorker_LongLegsEvent : IncidentWorker
    {
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(VariousDefOf.OCARINA_DaddyLongLegsRace))
			{
				return false;
			}
            return TryFindEntryCell(map, out IntVec3 cell);
        }

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!TryFindEntryCell(map, out var cell))
			{
				return false;
			}
			int chance = Rand.RangeInclusive(0, 100);
			PawnKindDef longlegs = VariousDefOf.OCARINA_BrotherLongLegsPawn;
			if (chance < 75)
				longlegs = VariousDefOf.OCARINA_BrotherLongLegsPawn;
			int value = 1;
			int num = Rand.RangeInclusive(90000, 150000);
            if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(cell, map, 10f, out IntVec3 result))
            {
                result = IntVec3.Invalid;
            }
            Pawn pawn = null;
			for (int i = 0; i < value; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(cell, map, 10);
				pawn = PawnGenerator.GeneratePawn(longlegs);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num;
				if (result.IsValid)
				{
					pawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(result, map, 10);
				}
			}
			SendStandardLetter("OCARINA_LetterLabelLongLegsPass".Translate(longlegs.label).CapitalizeFirst(), "OCARINA_LetterLongLegsPass".Translate(longlegs.label), LetterDefOf.ThreatBig, parms, pawn);
			return true;
		}

		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f);
		}
	}
}
