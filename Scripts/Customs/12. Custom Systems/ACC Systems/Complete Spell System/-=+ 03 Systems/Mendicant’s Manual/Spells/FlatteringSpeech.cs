using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;


namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class FlatteringSpeech : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Flattering Speech", "Linguam Laudamus",
                                                        // Custom spell effect parameters
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } } // 2 seconds delay
        public override double RequiredSkill { get { return 20.0; } } // Example required skill level
        public override int RequiredMana { get { return 12; } } // Mana cost of the spell

        public FlatteringSpeech(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FlatteringSpeech m_Owner;

            public InternalTarget(FlatteringSpeech owner) : base(3, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile && targeted is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    if (!creature.Controlled && creature.IsHumanInTown() && m_Owner.CheckSequence())
                    {
                        // Play sound and visual effects
                        from.PlaySound(0x24A); // Example sound for speech
                        Effects.SendTargetEffect(creature, 0x373A, 10, 30, 0x481, 0); // Example visual effect

                        // Chance to receive a gem
                        double chance = from.Skills[SkillName.Begging].Value / 100.0; // Higher begging skill increases chance
                        if (chance > Utility.RandomDouble())
                        {
                            Item gem = new Diamond(); // Assuming 'Gem' is a base item type representing various gems
                            from.AddToBackpack(gem);

                            from.SendMessage("Your flattering speech was successful! You received a gem.");
                        }
                        else
                        {
                            from.SendMessage("Your flattering speech failed to impress.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You cannot use Flattering Speech on this target.");
                    }
                }
                else
                {
                    from.SendMessage("You must target a suitable NPC.");
                }

                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
