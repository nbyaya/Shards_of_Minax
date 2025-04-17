using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.HealingMagic
{
    public class HealingSpellbook : CSpellbook
    {
        public override School School { get { return School.HealersLexicon; } }

        [Constructable]
        public HealingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public HealingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public HealingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1700;
            Name = "Healers Lexicon";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                {
                    return;
                }
            }

            // Update the spellbook content based on the HealingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (!profile.Talents.ContainsKey(TalentID.HealingSpells))
                    profile.Talents[TalentID.HealingSpells] = new Talent(TalentID.HealingSpells);
                this.Content = (ulong)profile.Talents[TalentID.HealingSpells].Points;
            }

            from.CloseGump(typeof(HealingSpellbookGump));
            from.SendGump(new HealingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new HealingSpellbookEntry(player, this));
            }
        }

        public HealingSpellbook(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Custom context menu entry to open the Healing Skill Tree.
    public class HealingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private HealingSpellbook m_Spellbook;

        public HealingSpellbookEntry(PlayerMobile player, HealingSpellbook spellbook)
            : base(1078990) // Use a custom Cliloc number for the entry text
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new HealingSkillTree(m_Player));
        }
    }
}
