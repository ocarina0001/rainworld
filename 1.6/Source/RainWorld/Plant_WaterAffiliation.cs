using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace RainWorld
{
    public class Plant_WaterAffiliation : Plant
    {
        const int radius = 12;
        bool waterFound = false;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            int num = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 cell = Position + GenRadial.RadialPattern[i];
                if (cell.InBounds(map))
                {
                    TerrainDef terrain = cell.GetTerrain(map);

                    if (terrain != null && terrain.IsWater)
                    {
                        waterFound = true;
                        break;
                    }
                }
            }
        }
        public override float GrowthRate
        {
            get
            {
                if (Blighted)
                {
                    return 0f;
                }
                if (Spawned && !PlantUtility.GrowthSeasonNow(Position, Map, def))
                {
                    return 0f;
                }
                return GrowthRateFactor_Fertility * GrowthRateFactor_Temperature * GrowthRateFactor_Light * GrowthRateFactor_Water * GrowthRateFactor_Rain;
            }
        }

        public float GrowthRateFactor_Water
        {
            get
            {
                if (waterFound)
                {
                    return 2f;
                }
                else return 1f;
            }
        }
        public float GrowthRateFactor_Rain
        {
            get
            {
                if (Map.weatherManager.RainRate>0)
                {
                    return 2f;
                }
                else return 1f;
            }
        }
    }
}
