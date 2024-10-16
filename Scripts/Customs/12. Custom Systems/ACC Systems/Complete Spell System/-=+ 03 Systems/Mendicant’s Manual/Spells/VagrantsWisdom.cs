using System;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class VagrantsWisdom : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Vagrants Wisdom", "Reveal Thoughts",
            21004, // Effect ID
            9300   // Gump ID or spellbook icon
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 18; } }

        public VagrantsWisdom(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private VagrantsWisdom m_Owner;

            public InternalTarget(VagrantsWisdom owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)targeted;
                    m_Owner.ShowThoughts(from, creature);
                }
                else
                {
                    from.SendMessage("You can only target a creature.");
                    m_Owner.DoFizzle();
                }
            }
        }

        public void ShowThoughts(Mobile caster, BaseCreature creature)
        {
            if (CheckSequence())
            {
                caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Visual effect around caster's head
                caster.PlaySound(0x1F7); // Casting sound

                List<string> thoughts = new List<string>
                {
                    "I wonder if anyone will ever understand me.",
                    "Life in this world is so fleeting...",
                    "The beauty of nature is unparalleled.",
                    "Every day is a new struggle.",
                    "I wish for peace, but prepare for war.",
                    "What lies beyond the horizon?",
                    "I'm just a shadow in the night.",
                    "This life... is it real or just a dream?"
                };

                string selectedThought = thoughts[Utility.Random(thoughts.Count)];

                caster.SendGump(new InnerThoughtsGump(creature, selectedThought));

                creature.Say("*thinks to themselves*"); // Creature says something
                creature.FixedParticles(0x375A, 1, 14, 9502, EffectLayer.Head); // Visual effect around creature's head
                creature.PlaySound(0x20F); // Sound indicating thought
            }

            FinishSequence();
        }

        private class InnerThoughtsGump : Gump
        {
            public InnerThoughtsGump(BaseCreature creature, string thought) : base(50, 50)
            {
                AddPage(0);
                AddBackground(0, 0, 300, 150, 5054);
                AddLabel(20, 20, 1153, "Inner Thoughts of " + creature.Name);
                AddHtml(20, 50, 260, 80, thought, false, false);
            }
        }
    }
}
