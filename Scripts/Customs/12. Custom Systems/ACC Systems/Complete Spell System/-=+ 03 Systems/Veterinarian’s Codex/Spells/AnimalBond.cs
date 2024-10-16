using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.VeterinaryMagic
{
    public class AnimalBond : VeterinarySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Bond", "Uus Ani",
            //SpellCircle.Third,
            21005,
            9301
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
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

            public InternalTarget(AnimalBond owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)o;

                    if (creature.Controlled && creature.ControlMaster == from)
                    {
                        if (m_Owner.CheckSequence())
                        {
                            creature.PlaySound(0x1F5); // Play a sound when the bond is strengthened
                            creature.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Display visual effects

                            // Apply temporary bonuses
                            creature.VirtualArmorMod += 10;
                            creature.Hits += 20;
                            creature.Stam += 20;

                            from.SendMessage("You strengthen your bond with your pet, enhancing its abilities.");

                            Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                            {
                                creature.VirtualArmorMod -= 10;
                                creature.Hits -= 20;
                                creature.Stam -= 20;

                                creature.PlaySound(0x1F8); // Sound when the bond effect ends
                                creature.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                                from.SendMessage("The bond with your pet returns to normal.");
                            });
                        }
                    }
                    else
                    {
                        from.SendMessage("You can only enhance the bond with your own controlled pet.");
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
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
