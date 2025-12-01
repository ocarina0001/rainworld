using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

public class BiomeWorker_Outskirts : BiomeWorker
{
    private Perlin perlin;

    private static readonly List<PlanetTile> NeighborList = new List<PlanetTile>();

    private const float ScarlandsThreshold = 0.9f;
    private const int SeedPart = 417235213;

    public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
    {
        if (tile.WaterCovered)
        {
            return -100f;
        }

        if (!AllowedAt(planetTile))
        {
            return 0f;
        }

        return 50f;
    }

    private bool AllowedAt(PlanetTile tile)
    {
        if (IsScarlandsEligible(tile))
        {
            return false;
        }

        Find.WorldGrid.GetTileNeighbors(tile, NeighborList);
        if (NeighborList.Empty())
        {
            return false;
        }

        foreach (PlanetTile n in NeighborList)
        {
            if (IsScarlandsEligible(n))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsScarlandsEligible(PlanetTile tile)
    {
        Tile t = Find.WorldGrid[tile];

        if (t.WaterCovered)
        {
            return false;
        }

        if (!BiomeWorker_TemperateForest.Allowed(t))
        {
            return false;
        }

        float n = NoiseAt(tile);
        if (n < ScarlandsThreshold)
        {
            return false;
        }

        Find.WorldGrid.GetTileNeighbors(tile, NeighborList);
        if (NeighborList.Empty())
        {
            return false;
        }

        foreach (PlanetTile neighbor in NeighborList)
        {
            if (NoiseAt(neighbor) >= ScarlandsThreshold)
            {
                return true;
            }
        }

        return false;
    }

    private float NoiseAt(PlanetTile tile)
    {
        if (perlin == null)
        {
            perlin = new Perlin(
                0.2,
                0.5,
                0.1,
                4,
                true,
                false,
                Gen.HashCombineInt(Find.World.info.Seed, SeedPart),
                QualityMode.Medium
            );
        }

        Vector3 c = Find.WorldGrid.GetTileCenter(tile);
        return (float)perlin.GetValue(c.x, c.y, c.z);
    }
}
