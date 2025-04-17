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

        public override SpellCircle Circle => SpellCircle.Third;
        public override double CastDelay => 0.5;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 20;

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

                    // Additional dramatic visuals
                    Effects.SendLocationParticles(EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                    Effects.PlaySound(trap.Location, trap.Map, 0x3B9);
                }
                else
                {
                    trap.TrapType = TrapType.None;
                    trap.TrapPower = 0;
                    Effects.SendLocationParticles(EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
                    Effects.PlaySound(trap.Location, trap.Map, 0x209);
                }

                trap.TrapMessage = 0;
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
                    from.SendLocalizedMessage(500237); // That is not a trap.
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

        [Constructable]
        public TrapableItem() : base(0x1EB0) // Replace with appropriate ItemID
        {
            TrapType = TrapType.None;
            TrapPower = 0;
            TrapMessage = 0;
        }

        public TrapableItem(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)TrapType);
            writer.Write(TrapPower);
            writer.Write(TrapMessage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            TrapType = (TrapType)reader.ReadInt();
            TrapPower = reader.ReadInt();
            TrapMessage = reader.ReadInt();
        }
    }

    public enum TrapType
    {
        None,
        Explosion,
        Poison,
        // Add additional trap types as needed
    }
}
