using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class SoulDrain : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Soul Drain", "Vita Nex",
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public SoulDrain(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private SoulDrain m_Owner;

            public InternalTarget(SoulDrain owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (m_Owner.CheckHSequence(target))
                    {
                        Mobile caster = m_Owner.Caster;

                        caster.PlaySound(0x1FB); // Sound effect for casting
                        Effects.SendLocationParticles(
                            EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                            0x3728,  // Particle effect ID
                            10,
                            30,
                            1149,
                            0, // Hue
                            0,
                            0x3F
                        );

                        double damage = Utility.RandomMinMax(10, 20); // Base damage
                        double drainAmount = damage * 0.5; // 50% of damage dealt is converted to health

                        // Apply damage to the target
                        target.Damage((int)damage, caster);

                        // Heal the caster by the drain amount
                        caster.Heal((int)drainAmount);

                        caster.SendMessage("You drain the soul of your enemy, restoring your own vitality!");
                        target.SendMessage("Your life force is being drained!");

                        // Additional visual effects on target
                        target.FixedParticles(0x374A, 10, 15, 5031, EffectLayer.Head);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
