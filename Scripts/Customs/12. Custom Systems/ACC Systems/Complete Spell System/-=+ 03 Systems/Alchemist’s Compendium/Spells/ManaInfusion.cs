using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class ManaInfusion : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mana Infusion", "Inficio Manae",
            266, // Circle (for visual representation)
            9040 // Sound effect
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 15; } }

        public ManaInfusion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (!CheckSequence())
                return;

            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            int manaToRestore = 20 + (int)(Caster.Skills[SkillName.Magery].Value * 0.2);
            target.Mana += manaToRestore;
            if (target.Mana > target.ManaMax)
                target.Mana = target.ManaMax;

            Effects.PlaySound(target.Location, target.Map, 0x1F2); // Sound effect for mana infusion
            target.FixedEffect(0x376A, 10, 16); // Visual effect for mana restoration

            Caster.SendMessage($"You have restored {manaToRestore} mana to {target.Name}.");
            target.SendMessage($"{Caster.Name} has restored {manaToRestore} mana to you.");
            
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ManaInfusion m_Owner;

            public InternalTarget(ManaInfusion owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
