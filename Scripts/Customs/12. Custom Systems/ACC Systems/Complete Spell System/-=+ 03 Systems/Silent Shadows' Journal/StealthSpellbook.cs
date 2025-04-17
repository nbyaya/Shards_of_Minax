using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.StealthMagic
{
    public class StealthSpellbook : CSpellbook
    {
        public override School School { get { return School.SilentShadowsJournal; } }

        [Constructable]
        public StealthSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public StealthSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public StealthSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2670;
            Name = "Silent Shadows Journal";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the spell content based on the StealthSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.StealthSpells].Points;
            }

            from.CloseGump(typeof(StealthSpellbookGump));
            from.SendGump(new StealthSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new StealthSpellbookEntry(player, this));
            }
        }

        public StealthSpellbook(Serial serial) : base(serial)
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

    // Context menu entry that opens the Stealth Skill Tree.
    public class StealthSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private StealthSpellbook m_Spellbook;

        public StealthSpellbookEntry(PlayerMobile player, StealthSpellbook spellbook)
            : base(1078990) // Use a custom cliloc id for "Skill Tree" if desired.
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new StealthSkillTree(m_Player));
        }
    }
}
