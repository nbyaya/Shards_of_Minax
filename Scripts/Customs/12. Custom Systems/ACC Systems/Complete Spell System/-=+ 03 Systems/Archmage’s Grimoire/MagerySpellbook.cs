using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class MagerySpellbook : CSpellbook
    {
        public override School School { get { return School.ArchmagesGrimoire; } }

        [Constructable]
        public MagerySpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public MagerySpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public MagerySpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 200;
            Name = "Archmages Grimoire";
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

            // Update content based on MagerySpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.MagerySpells].Points;
            }

            from.CloseGump(typeof(MagerySpellbookGump));
            from.SendGump(new MagerySpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new MagerySpellbookEntry(player, this));
            }
        }

        public MagerySpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry for the Magery Spellbook.
    public class MagerySpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private MagerySpellbook m_Spellbook;

        public MagerySpellbookEntry(PlayerMobile player, MagerySpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new MagerySkillTree(m_Player));
        }
    }
}
