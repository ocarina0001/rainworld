using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace RainWorld
{
    public class Plant_Ceiling : Plant
    {
        protected override bool HasEnoughLightToGrow
        {
            get
            {
				return true;
            }
        }

        protected override bool Resting
        {
            get
            {
				return false;
            }
        }

        public override float GrowthRate
        {
            get
            {
                if (Blighted)
                    return 0f;
                //Log.Error($"RAINWORLD: {GrowthRateFactor_Temperature + 1f}");
                return GrowthRateFactor_Temperature; 
            }
        }
    }
}
