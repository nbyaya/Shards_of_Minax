using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class ManaRestoration : MeditationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mana Restoration", "Mana Reficiat",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public ManaRestoration(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (Caster == target || Caster.CanBeBeneficial(target, true) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                
                SpellHelper.Turn(Caster, target);

                double meditationSkill = Caster.Skills[SkillName.Meditation].Value;
                int manaToRestore = (int)(meditationSkill / 2.0); // Restore half the Meditation skill level as mana

                target.Mana += manaToRestore;

                // Visual and sound effects
                target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Flashy visual effect
                target.PlaySound(0x1F5); // Magical sound effect

                Caster.SendMessage("You restore " + manaToRestore + " mana to " + (target == Caster ? "yourself" : target.Name) + ".");
                if (target != Caster)
                    target.SendMessage(Caster.Name + " has restored " + manaToRestore + " mana to you.");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ManaRestoration m_Owner;

            public InternalTarget(ManaRestoration owner) : base(12, false, TargetFlags.Beneficial)
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
