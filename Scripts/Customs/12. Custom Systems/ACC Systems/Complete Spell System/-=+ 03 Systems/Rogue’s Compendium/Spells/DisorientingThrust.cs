using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class DisorientingThrust : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Disorienting Thrust", "Take this!",
            21004,  // Replace with actual type
            9300 // Mana cost
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 10; } }

        public DisorientingThrust(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private DisorientingThrust m_Owner;

            public InternalTarget(DisorientingThrust owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(from, target);

                        // Play visual and sound effects
                        Effects.SendTargetEffect(target, 0x3779, 10, 30);
                        Effects.PlaySound(target.Location, target.Map, 0x28E);

                        // Apply debuff: Reduce accuracy and evasion
                        TimeSpan duration = TimeSpan.FromSeconds(10.0); // Duration of debuff
                        int debuffAmount = -10; // Amount to reduce accuracy and evasion

                        target.AddStatMod(new StatMod(StatType.Dex, "DisorientingThrustDex", debuffAmount, duration));
                        target.AddStatMod(new StatMod(StatType.Int, "DisorientingThrustInt", debuffAmount, duration));

                        target.SendMessage("You feel disoriented and vulnerable!");

                        // Apply a negative effect on skills related to accuracy and evasion
                        target.SkillMods.Add(new TimedSkillMod(SkillName.Tactics, true, debuffAmount, duration));
                        target.SkillMods.Add(new TimedSkillMod(SkillName.Parry, true, debuffAmount, duration));
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
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
