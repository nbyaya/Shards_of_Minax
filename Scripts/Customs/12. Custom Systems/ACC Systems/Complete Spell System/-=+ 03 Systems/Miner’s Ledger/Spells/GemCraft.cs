using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class GemCraft : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Gem Craft", "Transform raw gems into useful items or jewelry with enhanced properties",
            21005,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override int RequiredMana => 30;

        public GemCraft(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (!CheckSequence())
            {
                return;
            }

            Caster.Target = new InternalTarget(this);
        }

        public void Target(Item gem)
        {
            if (gem == null || gem.Deleted)
            {
                Caster.SendMessage("The gem is not valid.");
                return;
            }

            if (!gem.IsChildOf(Caster.Backpack))
            {
                Caster.SendMessage("You must have the gem in your backpack.");
                return;
            }

            List<Type> possibleTransforms = new List<Type>
            {
                typeof(RandomSkillJewelryB), // Example transformed item
                typeof(RandomSkillJewelryA), // Example transformed item
                typeof(RandomSkillJewelryG) // Example transformed item
            };

            // Create a gump to let player select an item to transform the gem into
            Caster.SendGump(new GemCraftGump(Caster, gem, possibleTransforms));
        }

        private class InternalTarget : Target
        {
            private GemCraft m_Spell;

            public InternalTarget(GemCraft spell) : base(12, false, TargetFlags.None)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item gem && (gem is Ruby || gem is Diamond || gem is Emerald || gem is Sapphire))
                {
                    m_Spell.Target(gem);
                }
                else
                {
                    from.SendMessage("You must target a raw gem like a ruby, diamond, emerald, or sapphire.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }
    }

    public class GemCraftGump : Gump
    {
        private Mobile m_Caster;
        private Item m_Gem;
        private List<Type> m_PossibleTransforms;

        public GemCraftGump(Mobile caster, Item gem, List<Type> possibleTransforms) : base(150, 50)
        {
            m_Caster = caster;
            m_Gem = gem;
            m_PossibleTransforms = possibleTransforms;

            AddPage(0);

            AddBackground(0, 0, 200, 150 + (possibleTransforms.Count * 25), 5054);
            AddLabel(50, 10, 0, "Select Transformation");

            for (int i = 0; i < possibleTransforms.Count; i++)
            {
                Type transformType = possibleTransforms[i];
                AddButton(10, 40 + (i * 25), 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0);
                AddLabel(45, 40 + (i * 25), 0, transformType.Name);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Gem == null || m_Gem.Deleted || !m_Gem.IsChildOf(m_Caster.Backpack))
            {
                m_Caster.SendMessage("The gem is not valid or not in your backpack.");
                return;
            }

            int buttonId = info.ButtonID - 1;

            if (buttonId >= 0 && buttonId < m_PossibleTransforms.Count)
            {
                Type transformType = m_PossibleTransforms[buttonId];

                try
                {
                    Item transformedItem = (Item)Activator.CreateInstance(transformType);
                    if (transformedItem != null)
                    {
                        m_Gem.Delete(); // Consume the gem
                        m_Caster.AddToBackpack(transformedItem);
                        m_Caster.PlaySound(0x5C5); // Magic transformation sound
                        m_Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Head); // Visual effect
                        m_Caster.SendMessage("The gem has been transformed into a " + transformType.Name + "!");
                    }
                }
                catch (Exception ex)
                {
                    m_Caster.SendMessage("An error occurred while transforming the gem: " + ex.Message);
                }
            }
        }
    }
}
