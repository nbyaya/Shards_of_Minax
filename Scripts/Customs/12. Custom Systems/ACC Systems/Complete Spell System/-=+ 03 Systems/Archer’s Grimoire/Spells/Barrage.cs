using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class Barrage : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Barrage", "Ex Plaga Fletus",
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 75.0; } }
        public override int RequiredMana { get { return 40; } }

        public Barrage(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private Barrage m_Owner;

            public InternalTarget(Barrage owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (!from.CanBeHarmful(target))
                    {
                        from.SendLocalizedMessage(100001); // That cannot be harmed.
                        return;
                    }

                    from.RevealingAction();
                    from.DoHarmful(target);

                    Effects.SendTargetParticles(target, 0x36BD, 20, 10, 1153, 0, 0, EffectLayer.Waist, 0);
                    Effects.PlaySound(target.Location, target.Map, 0x145);

                    int damage = Utility.RandomMinMax(30, 40); // Base damage range
                    double skillMultiplier = from.Skills[SkillName.Archery].Value / 100.0;
                    damage = (int)(damage * (1 + skillMultiplier));

                    AOS.Damage(target, from, damage, 100, 0, 0, 0, 0); // All damage is physical

                    for (int i = 0; i < 3; i++)
                    {
                        Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), () =>
                        {
                            if (!target.Deleted && target.Alive)
                            {
                                Effects.SendTargetParticles(target, 0x36BD, 20, 10, 1153, 0, 0, EffectLayer.Waist, 0);
                                Effects.PlaySound(target.Location, target.Map, 0x145);
                                AOS.Damage(target, from, damage, 100, 0, 0, 0, 0);
                            }
                        });
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target cannot be seen.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
