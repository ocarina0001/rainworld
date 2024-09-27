using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace RainWorld
{
	public class HediffCompProperties_CauseMentalState : HediffCompProperties
	{
		public MentalStateDef animalMentalState;

		public MentalStateDef animalMentalStateAlias;

		public MentalStateDef humanMentalState;

		public float mtbDaysToCauseMentalState;

		public bool endMentalStateOnCure = true;

		public HediffCompProperties_CauseMentalState()
		{
			compClass = typeof(HediffComp_CauseMentalState);
		}
	}
}
