using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class ChivalrySpellbook2 : CSpellbook
    {
        public override School School { get { return School.PaladinsTestament; } }

        [Constructable]
        public ChivalrySpellbook2() : this((ulong)0, CSSettings.FullSpellbooks) { }

        [Constructable]
        public ChivalrySpellbook2(bool full) : this((ulong)0, full) { }

        [Constructable]
        public ChivalrySpellbook2(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2110;
            Name = "Paladin's Testament";
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

            // Update the content based on the ChivalrySpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.ChivalrySpells].Points;
            }

            from.CloseGump(typeof(ChivalrySpellbook2Gump));
            from.SendGump(new ChivalrySpellbook2Gump(this));
        }

        public ChivalrySpellbook2(Serial serial) : base(serial) { }

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

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new ChivalrySpellbookEntry(player, this));
            }
        }
    }

    // Context menu entry that opens the Chivalry Skill Tree.
    public class ChivalrySpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private ChivalrySpellbook2 m_Spellbook;

        public ChivalrySpellbookEntry(PlayerMobile player, ChivalrySpellbook2 spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new ChivalrySkillTree(m_Player));
        }
    }
}
