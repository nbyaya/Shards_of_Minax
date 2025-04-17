using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.AnimalTamingMagic
{
    public class AnimalTamingSpellbook : CSpellbook
    {
        public override School School { get { return School.BeastmastersCodex; } }

        [Constructable]
        public AnimalTamingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public AnimalTamingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public AnimalTamingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 400;
            Name = "Beastmasters Codex";
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

            // Update the content based on the AnimalTamingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.AnimalTamingSpells].Points;
            }

            from.CloseGump(typeof(AnimalTamingSpellbookGump));
            from.SendGump(new AnimalTamingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new AnimalTamingSpellbookEntry(player, this));
            }
        }

        public AnimalTamingSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Animal Taming Skill Tree.
    public class AnimalTamingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private AnimalTamingSpellbook m_Spellbook;

        public AnimalTamingSpellbookEntry(PlayerMobile player, AnimalTamingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new AnimalTamingSkillTree(m_Player));
        }
    }
}
