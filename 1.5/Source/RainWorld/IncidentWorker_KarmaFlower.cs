using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace RainWorld
{
    public class IncidentWorker_KarmaFlower : IncidentWorker
    {
		private static int CountRange = 1;

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			if (!map.weatherManager.growthSeasonMemory.GrowthSeasonOutdoorsNow)
			{
				return false;
			}
			//Log.Message("RAINWORLD: Can fire!");
			return true;
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!TryFindRootCell(map, out var cell))
			{
				return false;
			}
			Thing thing = null;
			for (int i = 0; i < CountRange; i++)
			{
				if (!CellFinder.TryRandomClosewalkCellNear(cell, map, 6, out var result, (IntVec3 x) => CanSpawnAt(x, map)))
				{
					break;
				}
				result.GetPlant(map)?.Destroy();
				Thing thing2 = GenSpawn.Spawn(VariousDefOf.OCARINA_Plant_KarmaFlower, result, map);
				if (thing == null)
				{
					thing = thing2;
				}
			}
			if (thing == null)
			{
				return false;
			}
			SendStandardLetter(parms, thing);
			return true;
		}

		private bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => CanSpawnAt(x, map) && x.GetRoom(map).CellCount >= 64, map, out cell);
		}

		private bool CanSpawnAt(IntVec3 c, Map map)
		{
			//Log.Message($"RAINWORLD: Cell: {c} | Map: {map}");
			if (!c.Standable(map) || c.Fogged(map) || map.fertilityGrid.FertilityAt(c) < VariousDefOf.OCARINA_Plant_KarmaFlower.plant.fertilityMin || !c.GetRoom(map).PsychologicallyOutdoors || c.GetEdifice(map) != null || !PlantUtility.GrowthSeasonNow(c, map))
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == VariousDefOf.OCARINA_Plant_KarmaFlower)
				{
					return false;
				}
			}
			//Log.Message("RAINWORLD: Can spawn!");
			return true;
		}
	}
}
