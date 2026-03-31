using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace RainWorld
{
    public class JobDriver_Maul : JobDriver
    {
        private Pawn Target => (Pawn)job.targetA.Thing;
        private const int HitInterval = 10;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(() => Target.Dead);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            int attackCount = Rand.Range(3, 12);
            for (int i = 0; i < attackCount; i++)
            {
                Toil attackToil = new Toil();
                attackToil.initAction = () =>
                {
                    Pawn caster = pawn;
                    Pawn target = Target;
                    if (caster == null || target == null) return;
                    Verb verb = caster.meleeVerbs.TryGetMeleeVerb(target);
                    if (verb == null) return;
                    float damage = verb.verbProps.AdjustedMeleeDamageAmount(verb, caster);
                    float armorPen = verb.verbProps.AdjustedArmorPenetration(verb, caster);
                    DamageDef damageDef = verb.verbProps.meleeDamageDef;
                    if (damage > 0f)
                    {
                        DamageInfo dinfo = new DamageInfo(damageDef, damage, armorPen, -1f, caster);
                        target.TakeDamage(dinfo);
                    }
                };
                attackToil.defaultCompleteMode = ToilCompleteMode.Delay;
                attackToil.defaultDuration = HitInterval;
                yield return attackToil;
            }
        }
    }
}