using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class ShadowStrike : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Shadow Strike", "Vas Kal An Mani In Corp Hur",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 10; } }

        public ShadowStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ShadowStrike m_Owner;

            public InternalTarget(ShadowStrike owner) : base(10, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (!from.CanBeHarmful(target))
                    {
                        from.SendMessage("You cannot attack that target.");
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        from.PlaySound(0x482); // Sound of striking from the shadows
                        from.FixedParticles(0x3728, 1, 13, 9911, 37, 7, EffectLayer.Waist); // Shadow effect on the caster
                        target.PlaySound(0x1F2); // Sound effect when the target is hit
                        target.FixedParticles(0x3779, 1, 15, 9915, 1107, 3, EffectLayer.Waist); // Visual effect on the target

                        int damage = Utility.RandomMinMax(20, 30); // Base damage with bonus
                        from.DoHarmful(target);
                        AOS.Damage(target, from, damage, 100, 0, 0, 0, 0); // Physical damage

                        target.Paralyze(TimeSpan.FromSeconds(2.0)); // Briefly stun the target
                        from.SendMessage("You strike from the shadows, dealing extra damage and stunning your target!");

                        // Special visuals and sounds for a critical strike
                        if (Utility.RandomDouble() < 0.1) // 10% chance for a critical effect
                        {
                            target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                            target.PlaySound(0x214);
                            from.SendMessage("Critical strike!");
                        }
                    }
                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
