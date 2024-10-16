using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class AnimalBond : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Bond", "Mula Ostendo",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public AnimalBond(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AnimalBond m_Owner;

            public InternalTarget(AnimalBond owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature creature)
                {
                    if (creature.Controlled && creature.ControlMaster == from)
                    {
                        if (!creature.IsBonded)
                        {
                            creature.IsBonded = true;
                            creature.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist); // Bonding visual effect
                            creature.PlaySound(0x214); // Bonding sound effect
                            from.SendMessage("Your bond with the creature has been strengthened!");

                            m_Owner.FinishSequence();
                        }
                        else
                        {
                            from.SendMessage("This creature is already bonded.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You can only bond with your own tamed followers.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
