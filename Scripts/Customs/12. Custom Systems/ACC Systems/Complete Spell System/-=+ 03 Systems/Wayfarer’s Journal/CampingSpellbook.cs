using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class CampingSpellbook : CSpellbook
    {
        public override School School { get { return School.WayfarersJournal; } }

        [Constructable]
        public CampingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public CampingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public CampingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2739;
            Name = "Wayfarers Journal";
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

            // Update the content based on the CampingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.CampingSpells].Points;
            }

            from.CloseGump(typeof(CampingSpellbookGump));
            from.SendGump(new CampingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new CampingSpellbookEntry(player, this));
            }
        }

        public CampingSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry for CampingSpellbook to open the Camping Skill Tree.
    public class CampingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private CampingSpellbook m_Spellbook;

        public CampingSpellbookEntry(PlayerMobile player, CampingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree (adjust as needed)
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new CampingSkillTree(m_Player));
        }
    }
}
