using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class FishingSpear : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fishing Spear", "Aqua Telum",
            //SpellCircle.Fourth,
            //SpellCircle.Fifth,
            266,
            0x1C
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public FishingSpear(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FishingSpear m_Owner;

            public InternalTarget(FishingSpear owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(target, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                // Play visual and sound effects
                Effects.SendMovingParticles(Caster, target, 0x1FDD, 7, 0, false, true, 1153, 0, 9501, 1, 0, EffectLayer.Waist, 0x100);
                Effects.PlaySound(target.Location, target.Map, 0x026);

                // Calculate water-based damage
                int damage = Utility.RandomMinMax(15, 25) + (int)(Caster.Skills[SkillName.Fishing].Value / 5);

                // Inflict damage to target
                SpellHelper.Damage(this, target, damage, 0, 0, 100, 0, 0);

                // Add a brief water effect on the target
                target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Waist);
                target.PlaySound(0x026);
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
