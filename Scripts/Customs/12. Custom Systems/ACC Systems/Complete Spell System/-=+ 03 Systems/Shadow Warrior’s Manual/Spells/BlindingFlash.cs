using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Misc;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class BlindingFlash : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Blinding Flash", "Lux Illumino",
            21004,
            9300,
            false,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public BlindingFlash(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BlindingFlash m_Owner;

            public InternalTarget(BlindingFlash owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.CanBeHarmful(target))
                {
                    m_Owner.Target(target);
                }
            }
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanBeHarmful(target))
                return;

            if (CheckSequence())
            {
                Caster.DoHarmful(target);
                SpellHelper.Turn(Caster, target);

                // Play the flash effect
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 1153);
                target.PlaySound(0x1FB); // Flash sound

                // Apply blinding debuff
                TimeSpan duration = TimeSpan.FromSeconds(5.0 + (Caster.Skills[SkillName.Magery].Value * 0.05)); // Duration scales with caster's skill
                target.AddStatMod(new StatMod(StatType.Dex, "Blinded", -20, duration));
                target.AddStatMod(new StatMod(StatType.Int, "Confused", -20, duration));
                target.SendMessage("You are temporarily blinded and confused!");

                // Reduce hit chance
                target.MeleeDamageAbsorb = 10; // Reduce target's hit chance temporarily
                Timer.DelayCall(duration, () =>
                {
                    target.SendMessage("You regain your senses.");
                });
            }

            FinishSequence();
        }
    }
}
