using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class NinjitsuSpellbook : CSpellbook
    {
        public override School School { get { return School.ShadowWarriorsManual; } }

        [Constructable]
        public NinjitsuSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public NinjitsuSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public NinjitsuSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2440;
            Name = "Shadow Warriors Manual";
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

            // Update the content based on the NinjitsuSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.NinjitsuSpells].Points;
            }

            from.CloseGump(typeof(NinjitsuSpellbookGump));
            from.SendGump(new NinjitsuSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new NinjitsuSpellbookEntry(player, this));
            }
        }

        public NinjitsuSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry for the Ninjitsu Spellbook.
    public class NinjitsuSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private NinjitsuSpellbook m_Spellbook;

        public NinjitsuSpellbookEntry(PlayerMobile player, NinjitsuSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree entry
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new NinjitsuSkillTree(m_Player));
        }
    }
}
