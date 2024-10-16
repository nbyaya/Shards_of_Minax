using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class EmpathicTouch : WrestlingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Empathic Touch", "Empathic Heal",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 15; } }

        public EmpathicTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EmpathicTouch m_Owner;

            public InternalTarget(EmpathicTouch owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckSequence())
                    {
                        int healAmount = Utility.RandomMinMax(30, 50); // Medium amount of healing

                        // Apply healing to the target
                        target.Heal(healAmount);

                        // Visual and sound effects for the healing
                        target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Healing particles
                        target.PlaySound(0x202); // Healing sound

                        // Display a message to the caster
                        from.SendMessage($"You heal {target.Name} for {healAmount} hit points!");
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
