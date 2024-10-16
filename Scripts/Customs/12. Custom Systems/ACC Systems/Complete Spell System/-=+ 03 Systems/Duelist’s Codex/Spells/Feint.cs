using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class Feint : FencingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Feint", "Reducere Armor", 
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; } // Adjust based on your spell circle hierarchy
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } } // Adjust as needed
        public override int RequiredMana { get { return 20; } }

        public Feint(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Feint m_Owner;

            public InternalTarget(Feint owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (!m_Owner.CheckHSequence(target))
                        return;

                    m_Owner.Caster.DoHarmful(target);

                    // Lower armor effect
                    target.SendMessage("Your armor is severely weakened!");
                    target.PlaySound(0x1FB);
                    target.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist);

                    // Temporarily reduce target's armor
                    double originalArmor = target.ArmorRating;
                    target.VirtualArmorMod -= (int)(originalArmor * 0.9); // Reduce armor by 90%

                    Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                    {
                        // Restore armor after duration
                        if (target != null && !target.Deleted && target.Alive)
                        {
                            target.VirtualArmorMod += (int)(originalArmor * 0.9); // Restore armor
                            target.SendMessage("Your armor has returned to its normal strength.");
                            target.PlaySound(0x1F7);
                            target.FixedParticles(0x375A, 10, 15, 5036, EffectLayer.Waist);
                        }
                    });
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
