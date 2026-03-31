using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RainWorld
{
    [HarmonyPatch(typeof(PawnRenderUtility), "DrawEquipmentAndApparelExtras")]
    public static class Patch_PawnRenderUtility_DrawEquipmentAndApparelExtras
    {
        private static bool Prefix(Vector3 drawPos, Rot4 facing, PawnRenderFlags flags, Pawn pawn)
        {
            if (pawn == null || pawn.Dead || pawn.Downed || !pawn.RaceProps.Animal)
                return true;
            var c = pawn.TryGetComp<CompEquipWeapon>();
            if (c == null || c.cachedWeapon == null)
                return true;
            bool shouldDraw =
                pawn.CurJobDef == JobDefOf.AttackMelee ||
                pawn.CurJobDef == VariousDefOf.OCARINA_RainWorldShootWeapon ||
                (pawn.playerSettings != null &&
                pawn.playerSettings.Master != null &&
                pawn.playerSettings.Master.Drafted &&
                pawn.playerSettings.followDrafted) ||
                pawn.mindState.mentalStateHandler.CurStateDef == MentalStateDefOf.Manhunter;
            if (!shouldDraw)
                return true;
            var busy = pawn.stances.curStance as Stance_Busy;
            if (pawn.CurJobDef == VariousDefOf.OCARINA_RainWorldShootWeapon && busy != null && !busy.neverAimWeapon && busy.focusTarg.IsValid)
            {
                Vector3 tgt = busy.focusTarg.HasThing ? busy.focusTarg.Thing.DrawPos : busy.focusTarg.Cell.ToVector3Shifted();
                float ang = 0f;
                if ((tgt - pawn.DrawPos).MagnitudeHorizontalSquared() > 0.001f)
                    ang = (tgt - pawn.DrawPos).AngleFlat();
                Vector3 loc = drawPos + new Vector3(0f, 0f, 0.4f).RotatedBy(ang);
                loc.y += 0.036734693f;
                DrawAnimalEquipmentAiming(c.cachedWeapon, loc, ang);
                return false;
            }
            if (pawn.Rotation == Rot4.South)
            {
                Vector3 loc = drawPos + new Vector3(0f, 0f, -0.2f);
                loc.y += 0.035f;
                DrawAnimalEquipmentAiming(c.cachedWeapon, loc, 143f);
            }
            else if (pawn.Rotation == Rot4.North)
            {
                Vector3 loc = drawPos + new Vector3(0f, 0f, -0.1f);
                DrawAnimalEquipmentAiming(c.cachedWeapon, loc, 143f);
            }
            else if (pawn.Rotation == Rot4.East)
            {
                Vector3 loc = drawPos + new Vector3(0.2f, 0f, -0.2f);
                loc.y += 0.035f;
                DrawAnimalEquipmentAiming(c.cachedWeapon, loc, 143f);
            }
            else if (pawn.Rotation == Rot4.West)
            {
                Vector3 loc = drawPos + new Vector3(-0.2f, 0f, -0.2f);
                loc.y += 0.035f;
                DrawAnimalEquipmentAiming(c.cachedWeapon, loc, 217f);
            }
            return false;
        }

        private static void DrawAnimalEquipmentAiming(Thing eq, Vector3 loc, float angle)
        {
            float a = angle - 90f;
            Mesh mesh;
            if (angle > 20f && angle < 160f)
            {
                mesh = MeshPool.plane10;
                a += eq.def.equippedAngleOffset;
            }
            else if (angle > 200f && angle < 340f)
            {
                mesh = MeshPool.plane10Flip;
                a -= 180f;
                a -= eq.def.equippedAngleOffset;
            }
            else
            {
                mesh = MeshPool.plane10;
                a += eq.def.equippedAngleOffset;
            }
            a %= 360f;
            var mat = eq.Graphic is Graphic_StackCount g ? g.SubGraphicForStackCount(1, eq.def).MatSingle : eq.Graphic.MatSingle;
            Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(a, Vector3.up), mat, 0);
        }
    }
}