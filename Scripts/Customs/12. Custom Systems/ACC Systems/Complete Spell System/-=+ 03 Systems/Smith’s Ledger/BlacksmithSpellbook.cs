using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.BlacksmithMagic
{
    public class BlacksmithSpellbook : CSpellbook
    {
        public override School School { get { return School.SmithsLedger; } }

        [Constructable]
        public BlacksmithSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public BlacksmithSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public BlacksmithSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1290;
            Name = "Smiths Ledger";
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

            // Update the content based on the BlacksmithSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.BlacksmithSpells].Points;
            }

            from.CloseGump(typeof(BlacksmithSpellbookGump));
            from.SendGump(new BlacksmithSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new BlacksmithSpellbookEntry(player, this));
            }
        }

        public BlacksmithSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Blacksmith Skill Tree.
    public class BlacksmithSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private BlacksmithSpellbook m_Spellbook;

        public BlacksmithSpellbookEntry(PlayerMobile player, BlacksmithSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new Server.ACC.CSS.Systems.BlacksmithMagic.BlacksmithSkillTree(m_Player));
        }
    }
}
