using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class ProvocationSpellbook : CSpellbook
    {
        public override School School { get { return School.InstigatorsTome; } }

        [Constructable]
        public ProvocationSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public ProvocationSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public ProvocationSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1900;
            Name = "Instigators Tome";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the content based on the ProvocationSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.ProvocationSpells].Points;
            }

            from.CloseGump(typeof(ProvocationSpellbookGump));
            from.SendGump(new ProvocationSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
                list.Add(new ProvocationSpellbookEntry(player, this));
        }

        public ProvocationSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry for ProvocationSpellbook.
    public class ProvocationSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private ProvocationSpellbook m_Spellbook;

        public ProvocationSpellbookEntry(PlayerMobile player, ProvocationSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new ProvocationSkillTree(m_Player));
        }
    }
}
