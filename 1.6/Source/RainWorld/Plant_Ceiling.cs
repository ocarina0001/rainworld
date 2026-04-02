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
                return this.Map?.roofGrid?.Roofed(this.Position) ?? false;
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
                if (Blighted) return 0f;
                if (!(this.Map?.roofGrid?.Roofed(this.Position) ?? false)) return 0f;
                return GrowthRateFactor_Temperature;
            }
        }

        public override string GetInspectString()
        {
            string baseString = base.GetInspectString();
            bool isRoofed = this.Map?.roofGrid?.Roofed(this.Position) ?? false;
            if (!isRoofed)
                baseString += "\n" + "RequiresRoofToGrow".Translate();
            return baseString;
        }
    }
}