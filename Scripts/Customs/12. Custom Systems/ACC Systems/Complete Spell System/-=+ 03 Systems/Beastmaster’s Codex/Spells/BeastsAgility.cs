using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class BeastsAgility : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Beast’s Agility", "Fortis Bestia",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public BeastsAgility(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BeastsAgility m_Owner;

            public InternalTarget(BeastsAgility owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    Mobile target = (Mobile)o;

                    if (target is BaseCreature && ((BaseCreature)target).ControlMaster == from)
                    {
                        m_Owner.ApplyEffect((BaseCreature)target);
                    }
                    else
                    {
                        from.SendMessage("You can only cast this spell on your tamed animals.");
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void ApplyEffect(BaseCreature target)
        {
            if (CheckSequence())
            {
                target.PlaySound(0x213); // Play a magical sound effect
                target.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist); // Show magical particles around the animal

                int originalDex = target.Dex;
                target.Dex += 50; // Increase Dex by 50

                Timer.DelayCall(TimeSpan.FromSeconds(30), () => 
                {
                    target.Dex = originalDex; // Revert Dex after 30 seconds
                    target.PlaySound(0x1F4); // Play a sound when the effect ends
                    target.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Waist); // Show effect ending particles
                });
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
