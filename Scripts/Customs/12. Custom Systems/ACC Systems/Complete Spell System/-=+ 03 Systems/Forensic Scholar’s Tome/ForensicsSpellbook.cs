using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server;
using Server.Gumps;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class ForensicsSpellbook : CSpellbook
    {
        public override School School { get { return School.ForensicScholarsTome; } }

        [Constructable]
        public ForensicsSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public ForensicsSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public ForensicsSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1400;
            Name = "Forensic Scholars Tome";
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

            // Update content based on ForensicSpells talent bits
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[(TalentID)TalentID.ForensicSpells].Points;
            }

            from.CloseGump(typeof(ForensicsSpellbookGump));
            from.SendGump(new ForensicsSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new ForensicsSpellbookEntry(player, this));
            }
        }

        public ForensicsSpellbook(Serial serial) : base(serial)
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

    // Custom context menu entry to open the Forensics Skill Tree.
    public class ForensicsSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private ForensicsSpellbook m_Spellbook;

        public ForensicsSpellbookEntry(PlayerMobile player, ForensicsSpellbook spellbook)
            : base(1078990) // Use an appropriate Cliloc ID or custom number
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;
            m_Player.SendGump(new ForensicsSkillTree(m_Player));
        }
    }
}
