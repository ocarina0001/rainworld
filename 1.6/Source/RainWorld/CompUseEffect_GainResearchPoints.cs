using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RainWorld
{
    class CompUseEffect_GainResearchPoints : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            ResearchProjectDef project = Find.ResearchManager.GetProject();
            if (project != null)
            {
                AddProgress(project, usedBy);
            }
        }

        public override AcceptanceReport CanBeUsedBy(Pawn p)
        {
            if (Find.ResearchManager.GetProject() == null)
            {
                return "NoActiveResearchProjectToFinish".Translate();
            }
            return true;
        }

        private void AddProgress(ResearchProjectDef proj, Pawn usedBy)
        {
            float added;

            if (Rand.Chance(0.5f))
            {
                float pct = Rand.Range(0.01f, 0.05f);
                added = proj.baseCost * pct;
            }
            else
                added = Rand.Range(10f, 300f);
            Find.ResearchManager.AddProgress(proj, added);
            Messages.Message("MessageResearchProgressAdded".Translate(proj.LabelCap, added.ToString("F0")), usedBy, MessageTypeDefOf.PositiveEvent);
        }
    }
}
