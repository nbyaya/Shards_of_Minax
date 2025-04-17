using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Commands;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class BeggingSpellbook : CSpellbook
    {
        public override School School { get { return School.MendicantsManual; } }

        [Constructable]
        public BeggingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public BeggingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public BeggingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1200;
            Name = "Mendicants Manual";
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

            // Update the spell content based on BeggingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.BeggingSpells].Points;
            }

            from.CloseGump(typeof(BeggingSpellbookGump));
            from.SendGump(new BeggingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new BeggingSpellbookEntry(player, this));
            }
        }

        public BeggingSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Begging Skill Tree.
    public class BeggingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private BeggingSpellbook m_Spellbook;

        public BeggingSpellbookEntry(PlayerMobile player, BeggingSpellbook spellbook)
            : base(1078990) // Use your custom cliloc or text identifier for "Begging Skill Tree"
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new BeggingSkillTree(m_Player));
        }
    }
}
