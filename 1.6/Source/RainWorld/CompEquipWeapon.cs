using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace RainWorld
{
    [StaticConstructorOnStartup]
    public class CompEquipWeapon : ThingComp
    {
        public List<Verb> meleeSet;
        public List<Verb> rangedSet;
        public ThingWithComps cachedWeapon;
        public string cachedWeaponId = "";

        private Pawn PawnRef => parent as Pawn;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref cachedWeaponId, "ocarinaCachedId", "");
        }

        public override void CompTick()
        {
            base.CompTick();
            if (PawnRef == null) return;

            if (cachedWeaponId != "" && cachedWeapon == null)
            {
                var eq = PawnRef.equipment?.AllEquipmentListForReading?.FirstOrDefault(t => t.ThingID == cachedWeaponId);
                if (eq != null)
                {
                    cachedWeapon = eq as ThingWithComps;
                    var comp = cachedWeapon.TryGetComp<CompEquippable>();

                    if (comp != null && !comp.AllVerbs.NullOrEmpty())
                    {
                        meleeSet = new List<Verb>();
                        rangedSet = new List<Verb>();

                        foreach (var v in comp.AllVerbs)
                        {
                            v.caster = PawnRef;
                            if (v.verbProps.IsMeleeAttack) meleeSet.Add(v);
                            else rangedSet.Add(v);
                        }
                    }
                }
            }

            if (cachedWeapon != null)
            {
                if (PawnRef.equipment == null ||
                    PawnRef.equipment.AllEquipmentListForReading.FirstOrDefault(t => t.ThingID == cachedWeapon.ThingID) == null)
                {
                    meleeSet = new List<Verb>();
                    rangedSet = new List<Verb>();
                    cachedWeapon = null;
                    cachedWeaponId = "";
                }
            }

            if (cachedWeapon == null || rangedSet.NullOrEmpty() || !CanShoot(PawnRef)) return;

            Pawn target = null;

            if ((PawnRef.CurJobDef == JobDefOf.AttackMelee || PawnRef.CurJobDef == JobDefOf.AttackStatic) &&
                PawnRef.CurJob.targetA.Thing is Pawn p1 &&
                GenSight.LineOfSightToEdges(PawnRef.Position, p1.Position, PawnRef.Map) &&
                (p1.Position - PawnRef.Position).LengthHorizontal <= rangedSet[0].verbProps.range)
            {
                target = p1;
            }

            if (target == null)
            {
                var cand = PawnRef.Map.mapPawns.AllPawnsSpawned.Where(p =>
                    !p.Dead &&
                    !p.Downed &&
                    (p.HostileTo(PawnRef) || PawnRef.HostileTo(p)) &&
                    (p.Position - PawnRef.Position).LengthHorizontal <= rangedSet[0].verbProps.range &&
                    GenSight.LineOfSightToEdges(PawnRef.Position, p.Position, PawnRef.Map));

                if (!cand.EnumerableNullOrEmpty()) target = cand.RandomElement();
            }

            if (target != null)
            {
                if ((target.Position - PawnRef.Position).LengthHorizontal < 2f)
                    PawnRef.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AttackMelee, target), JobTag.Misc);
                else
                    PawnRef.jobs.TryTakeOrderedJob(JobMaker.MakeJob(VariousDefOf.OCARINA_RainWorldShootWeapon, target), JobTag.Misc);
            }
        }

        private bool CanShoot(Pawn a)
        {
            if (a != null && a.RaceProps.Animal && a.Map != null)
            {
                if (a.playerSettings?.Master != null &&
                    a.playerSettings.Master.Drafted &&
                    a.playerSettings.followDrafted)
                    return a.CurJobDef != VariousDefOf.OCARINA_RainWorldShootWeapon;

                if (a.CurJobDef == JobDefOf.AttackMelee ||
                    a.CurJobDef == JobDefOf.AttackStatic ||
                    a.IsFighting())
                    return a.CurJobDef != VariousDefOf.OCARINA_RainWorldShootWeapon;
            }
            return false;
        }
    }

    public class CompProperties_EquipWeapon : CompProperties
    {
        public CompProperties_EquipWeapon()
        {
            compClass = typeof(CompEquipWeapon);
        }
    }
}
