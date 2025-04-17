using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Commands;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class DiscordanceSpellbook : CSpellbook
    {
        public override School School { get { return School.DiscordantCodex; } }

        [Constructable]
        public DiscordanceSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public DiscordanceSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public DiscordanceSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1300;
            Name = "Discordant Codex";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the content based on the DiscordanceSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                // The Content now reflects the bits set by unlocked Discordance nodes.
                this.Content = (ulong)profile.Talents[TalentID.DiscordanceSpells].Points;
            }

            from.CloseGump(typeof(DiscordanceSpellbookGump));
            from.SendGump(new DiscordanceSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
                list.Add(new DiscordanceSpellbookEntry(player, this));
        }

        public DiscordanceSpellbook(Serial serial) : base(serial)
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

    // Context Menu Entry for opening the Discordance Skill Tree
    public class DiscordanceSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private DiscordanceSpellbook m_Spellbook;

        public DiscordanceSpellbookEntry(PlayerMobile player, DiscordanceSpellbook spellbook)
            : base(1078990) // Custom cliloc id (same as in the Lumberjacking example)
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player != null && m_Spellbook != null)
                m_Player.SendGump(new DiscordanceSkillTree(m_Player));
        }
    }
}
