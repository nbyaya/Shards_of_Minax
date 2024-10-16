using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class PersuasiveAppeal : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Persuasive Appeal", "Alms For The Poor!",
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

        public PersuasiveAppeal(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PersuasiveAppeal m_Owner;

            public InternalTarget(PersuasiveAppeal owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile && targeted != from)
                {
                    Mobile target = (Mobile)targeted;

                    if (target is BaseCreature baseCreature && !(target is PlayerMobile) && target.Alive && baseCreature.ControlMaster == null)
                    {
                        m_Owner.Beg(target);
                    }
                    else
                    {
                        from.SendMessage("You can only use this on NPCs.");
                        m_Owner.FinishSequence();
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                    m_Owner.FinishSequence();
                }
            }
        }

        public void Beg(Mobile target)
        {
            if (!CheckSequence())
                return;

            // Animation and sound effect
            Caster.Animate(32, 5, 1, true, false, 0); // Animation for begging
            Caster.PlaySound(0x50); // Sound effect for begging

            // Visual effects for a successful appeal
            if (Utility.RandomDouble() < 0.3) // 30% chance
            {
                Effects.SendLocationParticles(
                    Caster,    // IEntity
                    0x373A,   // Effect ID
                    1,        // Speed
                    30,       // Duration
                    1153,     // Hue
                    0,        // Render mode
                    0,        // Effect layer
                    0         // Effect ID 2
                );

                target.PlaySound(0x5A); // Sound effect when giving item
                Caster.SendMessage("The NPC feels pity and gives you a gift!");

                // Randomly select a power scroll
                Item scroll = new PowerScroll(SkillName.Wrestling, 105 + Utility.Random(5));
                Caster.AddToBackpack(scroll);
            }
            else
            {
                Caster.SendMessage("The NPC refuses your appeal.");
            }

            FinishSequence();
        }
    }
}
