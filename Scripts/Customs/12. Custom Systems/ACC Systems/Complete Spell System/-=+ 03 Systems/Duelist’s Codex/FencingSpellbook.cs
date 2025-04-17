// File: Server/ACC/CSS/Systems/FencingMagic/FencingSpellbook.cs
using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FencingMagic
{
    public class FencingSpellbook : CSpellbook
    {
        public override School School { get { return School.DuelistsCodex; } }

        [Constructable]
        public FencingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public FencingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public FencingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1200;
            Name = "Duelists Codex";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the content based on the player's FencingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.FencingSpells].Points;
            }

            from.CloseGump(typeof(FencingSpellbookGump));
            from.SendGump(new FencingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new FencingSpellbookEntry(player, this));
            }
        }

        public FencingSpellbook(Serial serial) : base(serial)
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

    // Custom context menu entry that opens the Fencing skill tree.
    public class FencingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private FencingSpellbook m_Spellbook;

        public FencingSpellbookEntry(PlayerMobile player, FencingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new FencingSkillTree(m_Player));
        }
    }
}
