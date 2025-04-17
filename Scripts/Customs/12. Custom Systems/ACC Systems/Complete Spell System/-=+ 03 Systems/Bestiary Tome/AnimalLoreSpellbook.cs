using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class AnimalLoreSpellbook : CSpellbook
    {
        public override School School { get { return School.BestiaryTome; } }

        [Constructable]
        public AnimalLoreSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public AnimalLoreSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public AnimalLoreSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 600;
            Name = "Bestiary Tome";
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

            // Update the content based on the AnimalLoreSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.AnimalLoreSpells].Points;
            }

            from.CloseGump(typeof(AnimalLoreSpellbookGump));
            from.SendGump(new AnimalLoreSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new AnimalLoreSpellbookEntry(player, this));
            }
        }

        public AnimalLoreSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry for the spellbook.
    public class AnimalLoreSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private AnimalLoreSpellbook m_Spellbook;

        public AnimalLoreSpellbookEntry(PlayerMobile player, AnimalLoreSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new AnimalLoreSkillTree(m_Player));
        }
    }
}
