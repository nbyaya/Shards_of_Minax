using System;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Commands;
using Server.Gumps;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class RemoveTrapSpellbook : CSpellbook
    {
        public override School School { get { return School.TrapmastersCodex; } }

        [Constructable]
        public RemoveTrapSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public RemoveTrapSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public RemoveTrapSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2552;
            Name = "Trapmasters Codex";
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

            // Update the spell content based on the RemoveTrapSpells talent bits.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                // The Content property now reflects unlocked spell bits.
                if (profile.Talents.ContainsKey(TalentID.RemoveTrapSpells))
                    this.Content = (ulong)profile.Talents[TalentID.RemoveTrapSpells].Points;
            }

            from.CloseGump(typeof(RemoveTrapSpellbookGump));
            from.SendGump(new RemoveTrapSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new RemoveTrapSpellbookEntry(player, this));
            }
        }

        public RemoveTrapSpellbook(Serial serial) : base(serial)
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

    // Context Menu Entry to open the Remove Trap Skill Tree.
    public class RemoveTrapSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private RemoveTrapSpellbook m_Spellbook;

        public RemoveTrapSpellbookEntry(PlayerMobile player, RemoveTrapSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree (change the number if needed)
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new RemoveTrapSkillTree(m_Player));
        }
    }
}
