using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.MeditationMagic
{
    public class MeditationSpellbook : CSpellbook
    {
        public override School School { get { return School.MediatorsGuide; } }

        [Constructable]
        public MeditationSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public MeditationSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public MeditationSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2300;
            Name = "Sages Codex";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the content based on the MeditationSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.MeditationSpells].Points;
            }

            from.CloseGump(typeof(MeditationSpellbookGump));
            from.SendGump(new MeditationSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new MeditationSpellbookEntry(player, this));
            }
        }

        public MeditationSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Meditation Skill Tree.
    public class MeditationSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private MeditationSpellbook m_Spellbook;

        public MeditationSpellbookEntry(PlayerMobile player, MeditationSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree (adjust as needed)
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;
            m_Player.SendGump(new MeditationSkillTree(m_Player));
        }
    }
}
