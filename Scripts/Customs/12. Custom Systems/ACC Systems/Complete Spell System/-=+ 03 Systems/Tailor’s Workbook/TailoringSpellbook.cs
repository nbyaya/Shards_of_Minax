using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class TailoringSpellbook : CSpellbook
    {
        public override School School { get { return School.TailorsWorkbook; } }

        [Constructable]
        public TailoringSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public TailoringSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public TailoringSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1260;
            Name = "Tailors Workbook";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the spell content based on TailoringSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.TailoringSpells].Points;
            }

            from.CloseGump(typeof(TailoringSpellbookGump));
            from.SendGump(new TailoringSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new TailoringSpellbookEntry(player, this));
            }
        }

        public TailoringSpellbook(Serial serial) : base(serial)
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

    public class TailoringSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private TailoringSpellbook m_Spellbook;

        public TailoringSpellbookEntry(PlayerMobile player, TailoringSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new TailoringSkillTree(m_Player));
        }
    }
}
