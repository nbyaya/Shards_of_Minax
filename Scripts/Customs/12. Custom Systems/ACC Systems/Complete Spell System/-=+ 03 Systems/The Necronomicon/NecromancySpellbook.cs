using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class NecromancySpellbook : CSpellbook
    {
        public override School School { get { return School.TheNecronomicon; } }

        [Constructable]
        public NecromancySpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public NecromancySpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public NecromancySpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1546;
            Name = "The Necronomicon";
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

            // Update the spell content based on unlocked NecromancySpells.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.NecromancySpells].Points;
            }

            from.CloseGump(typeof(NecromancySpellbookGump));
            from.SendGump(new NecromancySpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new NecromancySpellbookEntry(player, this));
            }
        }

        public NecromancySpellbook(Serial serial) : base(serial)
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

    // Custom context menu entry to open the Necromancy Skill Tree.
    public class NecromancySpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private NecromancySpellbook m_Spellbook;

        public NecromancySpellbookEntry(PlayerMobile player, NecromancySpellbook spellbook)
            : base(1078990) // Use a custom Cliloc for the Skill Tree entry.
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new NecromancySkillTree(m_Player));
        }
    }
}
