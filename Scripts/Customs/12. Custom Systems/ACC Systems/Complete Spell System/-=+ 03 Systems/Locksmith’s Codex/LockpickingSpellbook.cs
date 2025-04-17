using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class LockpickingSpellbook : CSpellbook
    {
        public override School School { get { return School.LocksmithsCodex; } }

        [Constructable]
        public LockpickingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public LockpickingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public LockpickingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2000;
            Name = "Locksmiths Codex";
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

            // Update the content based on the LockpickingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.LockpickingSpells].Points;
            }

            from.CloseGump(typeof(LockpickingSpellbookGump));
            from.SendGump(new LockpickingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new LockpickingSpellbookEntry(player, this));
            }
        }

        public LockpickingSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Lockpicking Skill Tree.
    public class LockpickingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private LockpickingSpellbook m_Spellbook;

        public LockpickingSpellbookEntry(PlayerMobile player, LockpickingSpellbook spellbook)
            : base(1078990) // Use a custom Cliloc for the Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new LockpickingSkillTree(m_Player));
        }
    }
}
