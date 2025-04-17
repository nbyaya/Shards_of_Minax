using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class ArcherySpellbook : CSpellbook
    {
        public override School School { get { return School.ArchersGrimoire; } }

        [Constructable]
        public ArcherySpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public ArcherySpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public ArcherySpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 360;
            Name = "Archers Grimoire";
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

            // Update the content based on the player's ArcherySpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.ArcherySpells].Points;
            }

            from.CloseGump(typeof(ArcherySpellbookGump));
            from.SendGump(new ArcherySpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new ArcherySpellbookEntry(player, this));
            }
        }

        public ArcherySpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Archery Skill Tree.
    public class ArcherySpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private ArcherySpellbook m_Spellbook;

        public ArcherySpellbookEntry(PlayerMobile player, ArcherySpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new ArcherySkillTree(m_Player));
        }
    }
}
