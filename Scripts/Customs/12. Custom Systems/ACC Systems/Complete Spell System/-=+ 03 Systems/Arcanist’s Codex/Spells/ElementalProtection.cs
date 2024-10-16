using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items; // Import for BuffInfo

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ElementalProtection : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Elemental Protection", "Vita Elementum",
            266,
            9040
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ElementalProtection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ElementalProtection m_Owner;

            public InternalTarget(ElementalProtection owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (target == null || !target.Alive || !from.CanSee(target))
                    {
                        from.SendLocalizedMessage(500237); // Target cannot be seen.
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        ApplyProtection(target);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500234); // Target is not valid.
                }
            }

            private void ApplyProtection(Mobile target)
            {
                if (target == null || !target.Alive)
                    return;

                // Apply visual and sound effects
                Effects.PlaySound(target, target.Map, 0x1F2); // Protective sound
                target.FixedEffect(0x373A, 10, 16); // Shield visual effect

                // Apply elemental protection buff
                AddElementalProtectionBuff(target);

                // Set up a timer to remove protection after a period
                new ProtectionTimer(target).Start();
            }

            private void AddElementalProtectionBuff(Mobile target)
            {
                // Use a valid BuffIcon. Example: BuffIcon.MagicResistance
                BuffIcon buffIcon = BuffIcon.Bless; 

                // Apply the buff
                target.Send(new AddBuffPacket(target, buffIcon, 1151383, 0, new TextDefinition(1151383), TimeSpan.FromSeconds(30.0)));
            }

            private void RemoveElementalProtectionBuff(Mobile target)
            {
                BuffIcon buffIcon = BuffIcon.Bless; // Ensure consistency in BuffIcon usage

                // Remove the buff from the target
                target.Send(new RemoveBuffPacket(target, buffIcon));
            }

            private class ProtectionTimer : Timer
            {
                private Mobile m_Target;

                public ProtectionTimer(Mobile target) : base(TimeSpan.FromSeconds(30.0))
                {
                    m_Target = target;
                }

                protected override void OnTick()
                {
                    if (m_Target != null && m_Target.Alive)
                    {
                        // Remove protection
                        RemoveElementalProtectionBuff(m_Target);
                    }
                }

                private void RemoveElementalProtectionBuff(Mobile target)
                {
                    BuffIcon buffIcon = BuffIcon.Bless; // Ensure consistency in BuffIcon usage

                    // Remove the buff from the target
                    target.Send(new RemoveBuffPacket(target, buffIcon));
                }
            }
        }
    }
}
