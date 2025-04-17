using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.ContextMenus;  // <-- Added this line!
using Server.ACC.CSS.Systems.MiningMagic; // Ensure the MiningMagic namespace is referenced

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class MiningSpellbook : CSpellbook
    {
        public override School School { get { return School.MinersLedger; } } // Define an appropriate school for mining

        [Constructable]
        public MiningSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public MiningSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public MiningSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 555;
            Name = "Miners' Grimoire";
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

            // Update the content based on the MiningSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.MiningSpells].Points;
            }

            from.CloseGump(typeof(MiningSpellbookGump));
            from.SendGump(new MiningSpellbookGump(this));
        }

        // Use 'new' instead of override if the base does not declare this method as virtual.
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new MiningSpellbookEntry(player, this));
            }
        }

        public MiningSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Mining Skill Tree.
    public class MiningSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private MiningSpellbook m_Spellbook;

        public MiningSpellbookEntry(PlayerMobile player, MiningSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        // Use 'new' if the base ContextMenuEntry doesn't mark OnClick as virtual.
        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new MiningSkillTree(m_Player));
        }
    }
}
