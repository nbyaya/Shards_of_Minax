using System;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ACC.CSS.Systems.ArmsLoreMagic; // For the skill tree context menu entry
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.MartialManual
{
    public class MartialManualBook : CSpellbook
    {
        public override School School { get { return School.MartialManual; } }

        [Constructable]
        public MartialManualBook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public MartialManualBook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public MartialManualBook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2141;
            Name = "Martial Manual";
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

            // Update the content based on the MartialManualSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.MartialManualSpells].Points;
            }

            from.CloseGump(typeof(MartialManualGump));
            from.SendGump(new MartialManualGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new MartialManualSkillTreeEntry(player, this));
            }
        }

        public MartialManualBook(Serial serial) : base(serial)
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

    // Context Menu Entry to open the Arms Lore Skill Tree.
    public class MartialManualSkillTreeEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private MartialManualBook m_Book;

        public MartialManualSkillTreeEntry(PlayerMobile player, MartialManualBook book)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Book = book;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Book == null)
                return;

            m_Player.SendGump(new ArmsLoreSkillTree(m_Player));
        }
    }
}
