using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RainWorld
{
    public class BiomeWorker_Outskirts : BiomeWorker
    {
        private const float IdealTemperature = 12f;
        private const float IdealRainfall = 1000f;
        private const float TemperatureTolerance = 3f;
        private const float RainfallTolerance = 150f;
        private const float MaxScore = 30f;

        public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
        {
            if (tile.WaterCovered)
                return -100f;
            if (tile.temperature < -5f || tile.temperature > 30f)
                return 0f;
            if (tile.rainfall < 400f || tile.rainfall > 2000f)
                return 0f;
            float tempDiff = tile.temperature - IdealTemperature;
            float rainDiff = tile.rainfall - IdealRainfall;
            float tempFactor = Mathf.Exp(-0.5f * (tempDiff * tempDiff) / (TemperatureTolerance * TemperatureTolerance));
            float rainFactor = Mathf.Exp(-0.5f * (rainDiff * rainDiff) / (RainfallTolerance * RainfallTolerance));
            float score = MaxScore * tempFactor * rainFactor;
            return Mathf.Max(0f, score);
        }
    }
}