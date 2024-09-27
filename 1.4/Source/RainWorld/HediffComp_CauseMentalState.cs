using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace RainWorld
{
    class HediffComp_CauseMentalState : HediffComp
    {
		public HediffCompProperties_CauseMentalState Props => (HediffCompProperties_CauseMentalState)props;

		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!Pawn.IsHashIntervalTick(60))
			{
				return;
			}
			if (Pawn.RaceProps.Humanlike)
			{
				if (Pawn.mindState.mentalStateHandler.CurStateDef != Props.humanMentalState && Rand.MTBEventOccurs(Props.mtbDaysToCauseMentalState, 60000f, 60f) && Pawn.Awake() && Pawn.mindState.mentalStateHandler.TryStartMentalState(Props.humanMentalState, parent.def.LabelCap, forceWake: false, causedByMood: false, null, transitionSilently: true) && Pawn.Spawned)
				{
					return;
				}
			}
			else if (Pawn.RaceProps.Animal && Pawn.mindState.mentalStateHandler.CurStateDef != Props.animalMentalState && (Props.animalMentalStateAlias == null || Pawn.mindState.mentalStateHandler.CurStateDef != Props.animalMentalStateAlias) && Rand.MTBEventOccurs(Props.mtbDaysToCauseMentalState, 60000f, 60f) && Pawn.Awake() && Pawn.mindState.mentalStateHandler.TryStartMentalState(Props.animalMentalState, parent.def.LabelCap, forceWake: false, causedByMood: false, null, transitionSilently: true) && Pawn.Spawned)
			{
				return;
			}
		}

		public override void CompPostPostRemoved()
		{
			if (Props.endMentalStateOnCure && ((Pawn.RaceProps.Humanlike && Pawn.mindState.mentalStateHandler.CurStateDef == Props.humanMentalState) || (Pawn.RaceProps.Animal && (Pawn.mindState.mentalStateHandler.CurStateDef == Props.animalMentalState || Pawn.mindState.mentalStateHandler.CurStateDef == Props.animalMentalStateAlias))) && !Pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				Pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}
	}
}
