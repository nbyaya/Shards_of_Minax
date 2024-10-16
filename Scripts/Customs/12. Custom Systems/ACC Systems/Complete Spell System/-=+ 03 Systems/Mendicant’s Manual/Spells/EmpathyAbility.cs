using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class EmpathyAbility : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Empathy", "Beg with Compassion",
            21004,
            9300,
            Reagent.BlackPearl
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 15; } }

        public EmpathyAbility(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EmpathyAbility m_Owner;

            public InternalTarget(EmpathyAbility owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature && ((BaseCreature)targeted).Controlled == false)
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    if (m_Owner.CheckSequence())
                    {
                        m_Owner.Caster.Mana -= m_Owner.RequiredMana;

                        // Visual and Sound Effects
                        Effects.PlaySound(creature.Location, creature.Map, 0x1F7);
                        creature.FixedParticles(0x373A, 10, 15, 5013, EffectLayer.Waist);

                        // Apply the begging effect with a chance of increased loot
                        double successChance = m_Owner.Caster.Skills[SkillName.Begging].Value / 100.0;

                        if (Utility.RandomDouble() < successChance)
                        {
                            creature.Say("I feel sorry for you, here take this.");
                            creature.PackItem(new Gold(Utility.RandomMinMax(10, 100)));
                            creature.PackItem(Loot.RandomGem());
                        }
                        else
                        {
                            creature.Say("I have nothing for you, beggar.");
                        }
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendMessage("You must target an NPC that is not controlled by a player.");
                }
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
