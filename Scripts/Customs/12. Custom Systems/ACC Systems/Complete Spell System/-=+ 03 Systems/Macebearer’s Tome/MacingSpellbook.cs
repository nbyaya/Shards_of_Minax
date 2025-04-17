using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.MacingMagic
{
    public class MacingSpellbook : CSpellbook
    {
        public override School School { get { return School.MacebearersTome; } }

        [Constructable]
        public MacingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public MacingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public MacingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2100;
            Name = "Macebearers Tome";
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

            // Update the content based on the MacingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.MacingSpells].Points;
            }

            from.CloseGump(typeof(MacingSpellbookGump));
            from.SendGump(new MacingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new MacingSpellbookEntry(player, this));
            }
        }

        public MacingSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Macing Skill Tree.
    public class MacingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private MacingSpellbook m_Spellbook;

        public MacingSpellbookEntry(PlayerMobile player, MacingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new MacingSkillTree(m_Player));
        }
    }
}
