using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class AnimalEmpathy : AnimalTamingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Animal Empathy", "Enhance Beasts!",
            21004,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Second;
        public override double CastDelay => 1.5;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 20;

        public AnimalEmpathy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AnimalEmpathy m_Owner;

            public InternalTarget(AnimalEmpathy owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is BaseCreature target)
                {
                    m_Owner.ApplyEffect(target);
                }
                else
                {
                    from.SendMessage("You must target a creature.");
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
                // Apply effects to the target and nearby creatures
                List<Mobile> affectedCreatures = new List<Mobile>();
                Map map = target.Map;

                if (map != null)
                {
                    foreach (Mobile m in target.GetMobilesInRange(1))
                    {
                        if (m is BaseCreature && m != target)
                        {
                            affectedCreatures.Add(m);
                        }
                    }

                    // Add the target itself
                    affectedCreatures.Add(target);

                    foreach (Mobile m in affectedCreatures)
                    {
                        // Apply buffs to Str, Dex, Int
                        m.PlaySound(0x1E6);
                        m.FixedParticles(0x375A, 10, 15, 5013, EffectLayer.Waist);
                        m.SendMessage("You feel a surge of energy!");

                        m.Str += 10;
                        m.Dex += 10;
                        m.Int += 10;

                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                        {
                            m.Str -= 10;
                            m.Dex -= 10;
                            m.Int -= 10;
                            m.SendMessage("The effects of Animal Empathy fade away.");
                        });
                    }
                }

                FinishSequence();
            }
        }
    }
}
