using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class SpikedAssault : MacingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Spiked Assault", "BLEED!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Eighth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public SpikedAssault(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        public void Target(Mobile target)
        {
            if (target == null || !Caster.CanBeHarmful(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                Caster.DoHarmful(target);

                // Infusing the mace with spikes effect
                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 30, 10, 0, 0);
                Effects.PlaySound(target.Location, target.Map, 0x209);

                // Apply initial damage
                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(target, Caster, damage, 0, 100, 0, 0, 0);

                // Apply bleed effect
                target.SendMessage("You are bleeding from the spiked assault!");
                Timer.DelayCall(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0), 5, BleedTick, target);

                Caster.Mana -= RequiredMana;
            }

            FinishSequence();
        }

        private void BleedTick(object state)
        {
            if (state is Mobile target && target.Alive)
            {
                int bleedDamage = Utility.RandomMinMax(3, 5);
                AOS.Damage(target, null, bleedDamage, 0, 100, 0, 0, 0);
                target.PlaySound(0x19C);
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Waist);
            }
        }

        private class InternalTarget : Target
        {
            private SpikedAssault m_Owner;

            public InternalTarget(SpikedAssault owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
