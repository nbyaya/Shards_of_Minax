using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.SpiritSpeakMagic
{
    public class SpiritSpeakSpellbook : CSpellbook
    {
        public override School School { get { return School.SpiritSpeak; } }

        [Constructable]
        public SpiritSpeakSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public SpiritSpeakSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public SpiritSpeakSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 546;
            Name = "Summoners Tome";
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

            // Update the spell content based on unlocked SpiritSpeakSpells.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.SpiritSpeakSpells].Points;
            }

            from.CloseGump(typeof(SpiritSpeakSpellbookGump));
            from.SendGump(new SpiritSpeakSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new SpiritSpeakSpellbookEntry(player, this));
            }
        }

        public SpiritSpeakSpellbook(Serial serial) : base(serial)
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

    // Custom context menu entry to open the SpiritSpeak Skill Tree.
    public class SpiritSpeakSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private SpiritSpeakSpellbook m_Spellbook;

        public SpiritSpeakSpellbookEntry(PlayerMobile player, SpiritSpeakSpellbook spellbook)
            : base(1078990) // Use a custom Cliloc for the Skill Tree entry.
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new SpiritSpeakSkillTree(m_Player));
        }
    }
}
