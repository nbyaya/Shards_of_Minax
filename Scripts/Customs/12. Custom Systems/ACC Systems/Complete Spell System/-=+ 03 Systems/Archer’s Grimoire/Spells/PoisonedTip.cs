using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class PoisonedTip : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Poisoned Tip", "Tox Ictus",
                                                        //SpellCircle.Second,
                                                        21005,
                                                        9301
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public PoisonedTip(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PoisonedTip m_Owner;

            public InternalTarget(PoisonedTip owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckHSequence(target))
                    {
                        SpellHelper.Turn(from, target);

                        // Visual effects
                        Effects.SendMovingEffect(from, target, 0xF42, 10, 0, false, false, 1161, 0);
                        from.PlaySound(0x2B);

                        // Damage over time effect
                        Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 5, () =>
                        {
                            if (target.Alive && !target.IsDeadBondedPet)
                            {
                                int damage = Utility.RandomMinMax(3, 5);
                                target.Damage(damage, from);

                                // Poison effect visuals
                                target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                                target.PlaySound(0x1F9);
                            }
                        });
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
