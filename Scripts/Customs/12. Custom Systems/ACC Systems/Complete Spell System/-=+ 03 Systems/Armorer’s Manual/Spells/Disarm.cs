using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class Disarm : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disarm", "Velox Manus",
            21004,
            9300,
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 30; } }

        public Disarm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile m)
        {
            if (m == null || !Caster.CanBeHarmful(m) || m == Caster)
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (CheckSequence())
            {
                Caster.DoHarmful(m);
                SpellHelper.Turn(Caster, m);

                if (Utility.RandomDouble() < 0.5) // 50% chance to disarm
                {
                    Item weapon = m.FindItemOnLayer(Layer.OneHanded) ?? m.FindItemOnLayer(Layer.TwoHanded);

                    if (weapon != null)
                    {
                        m.AddToBackpack(weapon);
                        m.SendMessage("You have been disarmed!");
                        Caster.SendMessage("You successfully disarm your opponent!");

                        Effects.SendTargetEffect(m, 0x376A, 10, 16); // Sparkle effect on the target
                        Effects.PlaySound(m.Location, m.Map, 0x1F6); // Disarm sound effect
                    }
                    else
                    {
                        Caster.SendMessage("Your target has no weapon to disarm.");
                    }
                }
                else
                {
                    Caster.SendMessage("You failed to disarm your opponent.");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Disarm m_Owner;

            public InternalTarget(Disarm owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile m)
                {
                    m_Owner.Target(m);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
