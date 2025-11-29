using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RainWorld
{
	[HarmonyPatch(typeof(Pawn_MeleeVerbs))]
	[HarmonyPatch("TryMeleeAttack")]
	internal class Patch_Pawn_MeleeVerbs_TryMeleeAttack
	{
		private static bool Prefix(bool __result, Thing target, Pawn ___pawn, bool surpriseAttack = false)
		{
			CompEquipWeapon aMW_Comp_EquipMeleeWeapon = ___pawn.TryGetComp<CompEquipWeapon>();
			if (aMW_Comp_EquipMeleeWeapon != null && aMW_Comp_EquipMeleeWeapon.cachedWeapon != null && !aMW_Comp_EquipMeleeWeapon.meleeSet.NullOrEmpty())
			{
				if (___pawn.stances.FullBodyBusy)
					return true;
				aMW_Comp_EquipMeleeWeapon.meleeSet.RandomElement().TryStartCastOn(target, surpriseAttack);
				__result = true;
				return false;
			}
			return true;
		}
	}
}
