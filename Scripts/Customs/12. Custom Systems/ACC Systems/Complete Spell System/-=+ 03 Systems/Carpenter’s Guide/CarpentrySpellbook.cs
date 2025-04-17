using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class CarpentrySpellbook : CSpellbook
    {
        public override School School { get { return School.CarpentersGuide; } }

        [Constructable]
        public CarpentrySpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public CarpentrySpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public CarpentrySpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 700;
            Name = "Carpenters Guide";
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

            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.CarpentrySpells].Points;
            }

            from.CloseGump(typeof(CarpentrySpellbookGump));
            from.SendGump(new CarpentrySpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new CarpentrySpellbookEntry(player, this));
            }
        }

        public CarpentrySpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry for Carpentry Spellbook.
    public class CarpentrySpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private CarpentrySpellbook m_Spellbook;

        public CarpentrySpellbookEntry(PlayerMobile player, CarpentrySpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new CarpentrySkillTree(m_Player));
        }
    }
}
