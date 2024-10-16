using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class QuickRelease : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Quick Release", "Releaso!",
            21001,
            9200,
            false,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public QuickRelease(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(TrapableItem trap)
        {
            if (trap == null || trap.TrapType == TrapType.None)
            {
                Caster.SendLocalizedMessage(500237); // That is not a trap.
            }
            else if (CheckSequence())
            {
                if (trap.TrapType == TrapType.Explosion || trap.TrapType == TrapType.Poison)
                {
                    Effects.SendLocationParticles(EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5013);
                    Effects.PlaySound(trap.Location, trap.Map, 0x1F1);
                    trap.TrapPower = (int)(trap.TrapPower * 0.5); // Reduce trap power by 50%

                    // Further reduce effects for dramatic visuals
                    Effects.SendLocationParticles(EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                    Effects.PlaySound(trap.Location, trap.Map, 0x3B9);
                }
                else
                {
                    trap.TrapType = TrapType.None; // Disable trap
                    trap.TrapPower = 0;
                    Effects.SendLocationParticles(EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    Effects.PlaySound(trap.Location, trap.Map, 0x209);
                }

                trap.TrapMessage = 0; // Remove trap message
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private QuickRelease m_Owner;

            public InternalTarget(QuickRelease owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is TrapableItem trap)
                {
                    m_Owner.Target(trap);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }

    public class TrapableItem : Item
    {
        public TrapType TrapType { get; set; }
        public int TrapPower { get; set; }
        public int TrapMessage { get; set; }

        // Constructor and other properties/methods
    }

    public enum TrapType
    {
        None,
        Explosion,
        Poison,
        // Add other types of traps if needed
    }
}
