using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class SilverTongueSpell : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Silver Tongue", "Hey buddy can you spare a copper?",
                                                        21004, 9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 16; } }

        public SilverTongueSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile target)
        {
            if (!Caster.CanSee(target) || !(target is BaseCreature))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, target);

                Effects.PlaySound(Caster.Location, Caster.Map, 0x2E6); // Play charming sound effect
                Caster.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Head); // Charm visual effect

                double chance = Caster.Skills[CastSkill].Value / 100.0; // Chance based on caster skill level
                int goldAmount = Utility.RandomMinMax(50, 200); // Random gold amount

                if (chance > Utility.RandomDouble()) // Successful persuasion
                {
                    Caster.SendMessage("Your silver tongue persuades the target to give you some gold!");
                    Caster.AddToBackpack(new Gold(goldAmount)); // Add gold to caster's backpack
                    target.Emote("*seems convinced and hands over some gold*");
                }
                else // Failed persuasion
                {
                    Caster.SendMessage("Your persuasion attempt fails.");
                    target.Emote("*looks unimpressed*");
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SilverTongueSpell m_Owner;

            public InternalTarget(SilverTongueSpell owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                    m_Owner.Target((Mobile)targeted);
                else
                    from.SendLocalizedMessage(500237); // Target can not be seen.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
