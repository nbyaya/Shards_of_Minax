using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.WrestlingMagic
{
    public class WrestlingSpellbook : CSpellbook
    {
        public override School School { get { return School.GrapplersManual; } }

        [Constructable]
        public WrestlingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public WrestlingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public WrestlingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1500;
            Name = "Grapplers Manual";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the content based on the WrestlingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.WrestlingSpells].Points;
            }

            from.CloseGump(typeof(WrestlingSpellbookGump));
            from.SendGump(new WrestlingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
                list.Add(new WrestlingSpellbookEntry(player, this));
        }

        public WrestlingSpellbook(Serial serial) : base(serial)
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

    // Context menu entry to open the Wrestling Skill Tree.
    public class WrestlingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private WrestlingSpellbook m_Spellbook;

        public WrestlingSpellbookEntry(PlayerMobile player, WrestlingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new WrestlingSkillTree(m_Player));
        }
    }
}
