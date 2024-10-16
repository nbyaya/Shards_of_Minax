using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class FrostShot : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Frost Shot", "Ex Glacies",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; } // Adjust as necessary
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public FrostShot(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FrostShot m_Owner;

            public InternalTarget(FrostShot owner) : base(10, false, TargetFlags.Harmful)
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
                return;
            }

            if (CheckHSequence(target))
            {
                SpellHelper.Turn(Caster, target);

                // Visual and sound effects
                Effects.SendTargetParticles(target, 0x3709, 10, 30, 5052, EffectLayer.Head);
                Effects.PlaySound(target.Location, target.Map, 0x64B);

                // Cold damage
                double damage = Utility.RandomMinMax(10, 30); // Adjust damage as necessary
                SpellHelper.Damage(this, target, damage, 0, 0, 100, 0, 0); // All damage is cold

                // Apply slow effect
                target.SendMessage("You feel a chilling cold slow your movements!");
                target.Paralyze(TimeSpan.FromSeconds(3.0)); // Adjust duration as necessary

                // Apply freeze visual effect
                target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            }

            FinishSequence();
        }
    }
}
